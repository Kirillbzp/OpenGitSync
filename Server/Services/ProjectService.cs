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
        ProjectDto CreateProject(ProjectDto projectDto);
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
            return projects.Select(p => MapToDto(p));
        }

        public ProjectDto GetProjectById(long id)
        {
            var project = _projectRepository.GetProjectById(id);
            return project != null ? MapToDto(project) : null;
        }

        public ProjectDto CreateProject(ProjectDto projectDto)
        {
            var project = MapToEntity(projectDto);
            var createdProject = _projectRepository.CreateProject(project);
            return MapToDto(createdProject);
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
            return MapToDto(updatedProject);
        }

        public ProjectDto DeleteProject(long id)
        {
            var existingProject = _projectRepository.GetProjectById(id);

            if (existingProject == null)
                return null;

            var deletedProject = _projectRepository.DeleteProject(existingProject);
            return MapToDto(deletedProject);
        }

        private ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                SyncSettings = project.SyncSettings.Select(s =>  _mapper.Map<SyncSettingDto>(s)).ToList()
            };
        }

        private Project MapToEntity(ProjectDto projectDto)
        {
            return new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                SyncSettings = projectDto.SyncSettings.Select(s => _mapper.Map<SyncSetting>(s)).ToList()
            };
        }

        public IEnumerable<SyncSettingDto> GetProjectSyncSettings(long projectId)
        {
            var syncSettings = _syncSettingRepository.GetSyncSettingsByProjectId(projectId);
            var syncSettingDtos = syncSettings.Select(syncSetting => new SyncSettingDto
            {
                // Map properties of SyncSetting entity to SyncSettingDto
                Id = syncSetting.Id,
                // Map other properties as needed
            }).ToList();

            return syncSettingDtos;
        }
    }

}
