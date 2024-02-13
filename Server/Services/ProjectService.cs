using AutoMapper;
using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IProjectService
    {
        IEnumerable<ProjectDto> GetProjects();
        ProjectDto GetProjectById(long id);
        ProjectDto CreateProject(CreateProjectDto projectDto);
        ProjectDto UpdateProject(long id, ProjectDto projectDto);
        ProjectDto DeleteProject(long id);
        IEnumerable<SyncSettingDto> GetProjectSyncSettings(long projectId);
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

        public IEnumerable<ProjectDto> GetProjects()
        {
            var projects = _projectRepository.GetProjects();
            return projects.Select(p => _mapper.Map<ProjectDto>(p));
        }

        public ProjectDto GetProjectById(long id)
        {
            var project = _projectRepository.GetProjectById(id);
            return project != null ? _mapper.Map<ProjectDto>(project) : null;
        }

        public ProjectDto CreateProject(CreateProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            var createdProject = _projectRepository.CreateProject(project);
            return _mapper.Map<ProjectDto>(createdProject);
        }

        public ProjectDto UpdateProject(long id, ProjectDto projectDto)
        {
            var existingProject = _projectRepository.GetProjectById(id);

            if (existingProject == null)
                return null;

            existingProject.Name = projectDto.Name;
            existingProject.Description = projectDto.Description;
            existingProject.SyncSettings = projectDto.SyncSettings.Select(s => _mapper.Map<SyncSetting>(s)).ToList();

            var updatedProject = _projectRepository.UpdateProject(existingProject);
            return _mapper.Map<ProjectDto>(updatedProject);
        }

        public ProjectDto DeleteProject(long id)
        {
            var existingProject = _projectRepository.GetProjectById(id);

            if (existingProject == null)
                return null;

            var deletedProject = _projectRepository.DeleteProject(existingProject);
            return _mapper.Map<ProjectDto>(deletedProject);
        }

        public IEnumerable<SyncSettingDto> GetProjectSyncSettings(long projectId)
        {
            var syncSettings = _syncSettingRepository.GetSyncSettingsByProjectId(projectId);
            var syncSettingDtos = syncSettings.Select(syncSetting => _mapper.Map<SyncSettingDto>(syncSetting)).ToList();

            return syncSettingDtos;
        }
    }

}
