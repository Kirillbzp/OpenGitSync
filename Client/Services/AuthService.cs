using OpenGitSync.Client.Providers;
using OpenGitSync.Shared.DataTransferObjects;
using OpenGitSync.Shared.ViewModels;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface IAuthService
    {
        public Task<string> AuthenticateAsync(string email, string password);
        public Task<ResponceResultDto> RegisterAsync(string username, string email, string password);
        public Task Logout();
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpPublicClient;
        private readonly ITokenService _tokenService;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

        public AuthService(IHttpClientFactory ClientFactory, 
                           CustomAuthenticationStateProvider customAuthenticationStateProvider,
                           ITokenService tokenService)
        {
            _httpPublicClient = ClientFactory.CreateClient("OpenGitSync.PublicServerAPI");
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
            _tokenService = tokenService;
        }

        public async Task<ResponceResultDto> RegisterAsync(string username, string email, string password)
        {
            var registrationDto = new UserRegistrationDto
            {
                UserName = username,
                Email = email,
                Password = password
            };

            var response = await _httpPublicClient.PostAsJsonAsync("/api/users/register", registrationDto);

            return await response.Content.ReadFromJsonAsync<ResponceResultDto>();
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var userLoginDto = new UserLoginDto
            {
                Email = email,
                Password = password
            };

            var response = await _httpPublicClient.PostAsJsonAsync("api/users/login", userLoginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                //Check if the login was successful
                if (result != null && result.Succeeded)
                {

                    //User login successful
                    await _tokenService.SetToken(result.Token);
                    _customAuthenticationStateProvider.StateChanged();

                    return "";
                }
                else
                {
                    //Display the error message from the API response
                    if (result == null)
                    {
                        return "General error";
                    }
                    else
                    {
                        return result.ErrorMessage;
                    }
                }
            }
            else
            {
                // User login failed, display a generic error message
                return "Invalid email or password.";
            }
        }

        public async Task Logout()
        {
            await _tokenService.RemoveToken();
            _customAuthenticationStateProvider.StateChanged();
        }   
    }
}
