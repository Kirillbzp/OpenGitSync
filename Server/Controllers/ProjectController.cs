using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/projects")]
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
        public IActionResult GetProjectById(long id)
        {
            var project = _projectService.GetProjectById(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public IActionResult CreateProject(ProjectDto projectDto)
        {
            var createdProject = _projectService.CreateProject(projectDto);

            return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(long id, ProjectDto projectDto)
        {
            var updatedProject = _projectService.UpdateProject(id, projectDto);

            if (updatedProject == null)
                return NotFound();

            return Ok(updatedProject);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(long id)
        {
            var deletedProject = _projectService.DeleteProject(id);

            if (deletedProject == null)
                return NotFound();

            return Ok(deletedProject);
        }
    }

}
