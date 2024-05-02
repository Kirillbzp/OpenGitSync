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
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetProjects();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(long id)
        {
            var project = await _projectService.GetProjectById(id);

            if (project == null)
                return NotFound(new { Error = "Project not found" });

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreateDto projectDto)
        {
            var createdProject = await _projectService.CreateProject(projectDto);

            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(long id, ProjectDto projectDto)
        {
            var updatedProject = await _projectService.UpdateProject(id, projectDto);

            if (updatedProject == null)
                return NotFound(new { Error = "Project not found" });

            return Ok(updatedProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(long id)
        {
            var deletedProject = await _projectService.DeleteProject(id);

            if (deletedProject == null)
                return NotFound(new { Error = "Project not found" });

            return Ok(deletedProject);
        }

        [HttpGet("{projectId}/sync-settings")]
        public async Task<IActionResult> GetProjectSyncSettings(long projectId)
        {
            // Retrieve the project sync settings from the service
            var syncSettings = await _projectService.GetProjectSyncSettings(projectId);

            if (syncSettings == null)
                return NotFound(new { Error = "Project not found" });

            // Return the project sync settings
            return Ok(syncSettings);
        }

    }

}
