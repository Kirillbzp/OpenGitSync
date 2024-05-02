using AutoMapper;
using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjects();
        Task<ProjectDto> GetProjectById(long id);
        Task<ProjectDto> CreateProject(ProjectCreateDto projectDto);
        Task<ProjectDto> UpdateProject(long id, ProjectDto projectDto);
        Task<ProjectDto> DeleteProject(long id);
        Task<IEnumerable<SyncSettingDto>> GetProjectSyncSettings(long projectId);
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

        public async Task<IEnumerable<ProjectDto>> GetProjects()
        {
            var projects = await _projectRepository.GetProjects();
            return projects.Select(p => _mapper.Map<ProjectDto>(p));
        }

        public async Task<ProjectDto> GetProjectById(long id)
        {
            var project = await _projectRepository.GetProjectById(id);
            return project != null ? _mapper.Map<ProjectDto>(project) : null;
        }

        public async Task<ProjectDto> CreateProject(ProjectCreateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            var createdProject = await _projectRepository.CreateProject(project);
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public async Task<ProjectDto> UpdateProject(long id, ProjectDto projectDto)
        {
            var existingProject = await _projectRepository.GetProjectById(id);

            if (existingProject == null)
                return null;

            existingProject.Name = projectDto.Name;
            existingProject.Description = projectDto.Description;
            existingProject.SyncSettings = projectDto.SyncSettings.Select(s => _mapper.Map<SyncSetting>(s)).ToList();

            var updatedProject = await _projectRepository.UpdateProject(existingProject);
            return _mapper.Map<ProjectDto>(updatedProject);
        }

        public async Task<ProjectDto> DeleteProject(long id)
        {
            var existingProject = await _projectRepository.GetProjectById(id);

            if (existingProject == null)
                return null;

            var deletedProject = await _projectRepository.DeleteProject(existingProject);
            return _mapper.Map<ProjectDto>(deletedProject);
        }

        public async Task<IEnumerable<SyncSettingDto>> GetProjectSyncSettings(long projectId)
        {
            var syncSettings = await _syncSettingRepository.GetSyncSettingsByProjectId(projectId);
            var syncSettingDtos = syncSettings.Select(syncSetting => _mapper.Map<SyncSettingDto>(syncSetting)).ToList();

            return syncSettingDtos;
        }
    }

}
