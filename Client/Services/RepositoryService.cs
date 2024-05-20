using OpenGitSync.Shared.DataTransferObjects;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface IRepositoryService
    {
        Task<List<RepositoryDto>> GetRepositories();
        Task<RepositoryDto> GetRepositoryById(string id);
        Task<List<RepositoryDto>> GetProjectRepositories(string projectId);
        Task<(bool, RepositoryDto)> CreateRepository(RepositoryCreateDto repositoryCreateDto);
        Task<bool> UpdateRepository(long id, RepositoryDto repositoryUpdateDto);
        Task<List<RepositoryDto>> RepositoryTypeahead(string query, long projectId);
        Task<bool> CheckConnection(string url, string token);
    }
}


namespace OpenGitSync.Client.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly HttpClient _httpClient;
        
        public RepositoryService(HttpClient httpClient, IHttpClientFactory clientFactory)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RepositoryDto>> GetRepositories()
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<List<RepositoryDto>>("api/repository");
            if (result == null)
            {
                return new List<RepositoryDto>();
            }
            return result;
        }

        public async Task<RepositoryDto> GetRepositoryById(string id)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<RepositoryDto>($"api/repository/{id}");
            if (result == null)
            {
                return new RepositoryDto();
            }
            return result;
        }

        public async Task<List<RepositoryDto>> GetProjectRepositories(string projectId)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<List<RepositoryDto>>($"api/repository/project/{projectId}");
            if (result == null)
            {
                return new List<RepositoryDto>();
            }
            return result;
        }

        public async Task<(bool, RepositoryDto)> CreateRepository(RepositoryCreateDto repositoryCreateDto)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/repository", repositoryCreateDto);
                var repository = await result.Content.ReadFromJsonAsync<RepositoryDto>();
                if (repository == null)
                {
                    return (false, null);
                }
                return (result.IsSuccessStatusCode, repository);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<bool> UpdateRepository(long id, RepositoryDto repositoryUpdateDto)
        {
            try
            {
                var result = await _httpClient.PutAsJsonAsync($"api/repository/{id}", repositoryUpdateDto);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<RepositoryDto>> RepositoryTypeahead(string query, long projectId)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<List<RepositoryDto>>($"api/repository/typeahead?query={query}&projectId={projectId}");
            if (result == null)
            {
                return new List<RepositoryDto>();
            }
            return result;
        }

        public async Task<bool> CheckConnection(string url, string token)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<bool>($"api/repository/checkConnection?url={url}&token={token}");
            return result;
        }
    }
}
