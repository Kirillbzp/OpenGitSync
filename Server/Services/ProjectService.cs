using AutoMapper;
using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjects(string userId);
        Task<ProjectDto?> GetProjectById(long id, string userId);
        Task<IdNameDto?> GetProjectNameById(long id, string userId);
        Task<ProjectDto> CreateProject(ProjectCreateDto projectDto, string userId);
        Task<ProjectDto?> UpdateProject(long id, ProjectDto projectDto, string userId);
        Task<ProjectDto?> DeleteProject(long id, string userId);
        Task<IEnumerable<SyncSettingDto>> GetProjectSyncSettings(long projectId, string userId);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ISyncSettingRepository _syncSettingRepository;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper, ISyncSettingRepository syncSettingRepository)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _syncSettingRepository = syncSettingRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjects(string userId)
        {
            var projects = await _projectRepository.GetProjects(userId);
            return projects.Select(p => _mapper.Map<ProjectDto>(p));
        }

        public async Task<ProjectDto?> GetProjectById(long id, string userId)
        {
            var project = await _projectRepository.GetProjectById(id, userId);
            return project != null ? _mapper.Map<ProjectDto>(project) : null;
        }

        public async Task<IdNameDto?> GetProjectNameById(long id, string userId)
        {
            var project = await _projectRepository.GetProjectById(id, userId);
            return project != null ? _mapper.Map<IdNameDto>(project) : null;
        }

        public async Task<ProjectDto> CreateProject(ProjectCreateDto projectDto, string userId)
        {
            var project = _mapper.Map<Project>(projectDto);
            var createdProject = await _projectRepository.CreateProject(project, userId);
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task<ProjectDto?> UpdateProject(long id, ProjectDto projectDto, string userId)
        {
            var existingProject = await _projectRepository.GetProjectById(id, userId);

            if (existingProject == null)
                return null;

            existingProject.Name = projectDto.Name;
            existingProject.Description = projectDto.Description;
            existingProject.SyncSettings = projectDto.SyncSettings.Select(s => _mapper.Map<SyncSetting>(s)).ToList();

            var updatedProject = await _projectRepository.UpdateProject(existingProject);
            return _mapper.Map<ProjectDto>(updatedProject);
        }

        public async Task<ProjectDto?> DeleteProject(long id, string userId)
        {
            var existingProject = await _projectRepository.GetProjectById(id, userId);

            if (existingProject == null)
                return null;

            var deletedProject = await _projectRepository.DeleteProject(existingProject);
            return _mapper.Map<ProjectDto>(deletedProject);
        }

        public async Task<IEnumerable<SyncSettingDto>> GetProjectSyncSettings(long projectId, string userId)
        {
            var syncSettings = await _syncSettingRepository.GetSyncSettingsByProjectId(projectId, userId);
            var syncSettingDtos = syncSettings.Select(syncSetting => _mapper.Map<SyncSettingDto>(syncSetting)).ToList();

            return syncSettingDtos;
        }
    }

}
