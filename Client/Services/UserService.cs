using OpenGitSync.Shared.DataTransferObjects;
using System.Net.Http;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfile();
        Task<bool> UpdateProfile(UserProfileDto UserProfileUpdateDto);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserProfileDto> GetProfile()
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<UserProfileDto>("/api/users/profile");
            if (result == null)
            {
                return new UserProfileDto();
            }
            return result;
        }
        public async Task<bool> UpdateProfile(UserProfileDto UserProfileUpdateDto)
        {             
            try
            {
                var result = await _httpClient.PutAsJsonAsync($"/api/users/profile", UserProfileUpdateDto);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
