using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OpenGitSync.Shared.DataTransferObjects;
using OpenGitSync.Shared.ViewModels;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface IAuthService
    {
        public Task<string> AuthenticateAsync(string email, string password);
        public Task<ResponceResultDto> RegisterAsync(string username, string email, string password);
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpPublicClient;
        private readonly ILocalStorageService _localStorage;
        public AuthService(IHttpClientFactory ClientFactory, ILocalStorageService localStorage)
        {
            _httpPublicClient = ClientFactory.CreateClient("OpenGitSync.PublicServerAPI");
            _localStorage = localStorage;
        }

        public async Task<ResponceResultDto> RegisterAsync (string username, string email, string password)
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
                // User login successful
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                // Check if the login was successful
                if (result != null && result.Succeeded)
                {
                    await _localStorage.SetItemAsync("token", result.Token);

                    return "";
                }
                else
                {
                    // Display the error message from the API response
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
    }
}
