using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OpenGitSync.Shared.DataTransferObjects;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Client.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjects();
        Task<ProjectDto> GetProjectById(string id);
        Task<ProjectDto> CreateProject(ProjectDto projectCreateDto);
        Task<ProjectDto> UpdateProject(string id, ProjectDto projectUpdateDto);
        Task<bool> DeleteProject(string id);
    }
}


namespace OpenGitSync.Client.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        
        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProjectDto>> GetProjects()
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects");
        }

        public async Task<ProjectDto> GetProjectById(string id)
        {
            return await _httpClient.GetFromJsonAsync<ProjectDto>($"api/projects/{id}");
        }

        public async Task<ProjectDto> CreateProject(ProjectDto projectCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", projectCreateDto);
            return await response.Content.ReadFromJsonAsync<ProjectDto>();
        }

        public async Task<ProjectDto> UpdateProject(string id, ProjectDto projectUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{id}", projectUpdateDto);
            return await response.Content.ReadFromJsonAsync<ProjectDto>();
        }

        public async Task<bool> DeleteProject(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
