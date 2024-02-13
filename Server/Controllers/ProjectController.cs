using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectController : ApiControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IApiControllerWrapper wrapper, IProjectService projectService) : base(wrapper)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = _projectService.GetProjects();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public IActionResult GetProject(long id)
        {
            var project = _projectService.GetProjectById(id);

            if (project == null)
                return NotFound(new { Error = "Project not found" });

            return Ok(project);
        }

        [HttpPost]
        public IActionResult CreateProject(CreateProjectDto projectDto)
        {
            var createdProject = _projectService.CreateProject(projectDto);

            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(long id, ProjectDto projectDto)
        {
            var updatedProject = _projectService.UpdateProject(id, projectDto);

            if (updatedProject == null)
                return NotFound(new { Error = "Project not found" });

            return Ok(updatedProject);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(long id)
        {
            var deletedProject = _projectService.DeleteProject(id);

            if (deletedProject == null)
                return NotFound(new { Error = "Project not found" });

            return Ok(deletedProject);
        }

        [HttpGet("{projectId}/sync-settings")]
        public IActionResult GetProjectSyncSettings(long projectId)
        {
            // Retrieve the project sync settings from the service
            var syncSettings = _projectService.GetProjectSyncSettings(projectId);

            if (syncSettings == null)
                return NotFound(new { Error = "Project not found" });

            // Return the project sync settings
            return Ok(syncSettings);
        }

    }

}
