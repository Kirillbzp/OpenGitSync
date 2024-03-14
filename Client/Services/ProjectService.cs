using OpenGitSync.Shared.DataTransferObjects;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjects();
        Task<ProjectDto> GetProjectById(string id);
        Task<(bool, ProjectDto)> CreateProject(CreateProjectDto projectCreateDto);
        Task<bool> UpdateProject(long id, ProjectDto projectUpdateDto);
    }
}


namespace OpenGitSync.Client.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        
        public ProjectService(HttpClient httpClient, IHttpClientFactory clientFactory)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProjectDto>> GetProjects()
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<List<ProjectDto>>("api/projects");
            if (result == null)
            {
                return new List<ProjectDto>();
            }
            return result;
        }

        public async Task<ProjectDto> GetProjectById(string id)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<ProjectDto>($"api/projects/{id}");
            if (result == null)
            {
                return new ProjectDto();
            }
            return result;
        }

        public async Task<(bool, ProjectDto)> CreateProject(CreateProjectDto projectCreateDto)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/projects", projectCreateDto);
                var project = await result.Content.ReadFromJsonAsync<ProjectDto>();
                if (project == null)
                {
                    return (false, null);
                }
                return (result.IsSuccessStatusCode, project);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<bool> UpdateProject(long id, ProjectDto projectUpdateDto)
        {
            try
            {
                var result = await _httpClient.PutAsJsonAsync($"api/projects/{id}", projectUpdateDto);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

    }
}
