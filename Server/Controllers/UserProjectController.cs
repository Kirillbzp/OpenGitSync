using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProjectController : ApiControllerBase
    {
        private readonly IUserProjectService _userProjectService;

        public UserProjectController(IApiControllerWrapper wrapper, IUserProjectService userProjectService) : base(wrapper)
        {
            _userProjectService = userProjectService;
        }

        [HttpPost]
        public ActionResult<UserProjectDto> AddUserToProject(UserProjectCreateDto createDto)
        {
            var userProject = MapCreateDtoToUserProject(createDto);

            _userProjectService.AddUserToProject(userProject);

            var userProjectDto = MapUserProjectToDto(userProject);
            return CreatedAtAction(nameof(GetUserProjectById), new { id = userProject.Id }, userProjectDto);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveUserFromProject(long id)
        {
            var userProject = _userProjectService.GetUserProjectById(id);
            if (userProject == null)
                return NotFound();

            _userProjectService.RemoveUserFromProject(userProject);

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<UserProjectDto> GetUserProjectById(long id)
        {
            var userProject = _userProjectService.GetUserProjectById(id);
            if (userProject == null)
                return NotFound();

            var userProjectDto = MapUserProjectToDto(userProject);
            return Ok(userProjectDto);
        }

        [HttpGet("project/{projectId}")]
        public ActionResult<IEnumerable<UserProjectDto>> GetUserProjectsByProjectId(long projectId)
        {
            var userProjects = _userProjectService.GetUserProjectsByProjectId(projectId);

            var userProjectDtos = userProjects.Select(up => MapUserProjectToDto(up));
            return Ok(userProjectDtos);
        }

        private UserProjectDto MapUserProjectToDto(UserProject userProject)
        {
            // Implement the mapping from UserProject entity to UserProjectDto
            // You can use a library like AutoMapper or manually map the properties
            // Here's an example using manual mapping:
            var userProjectDto = new UserProjectDto
            {
                Id = userProject.Id,
                UserId = userProject.UserId,
                ProjectId = userProject.ProjectId,
                Role = userProject.Role
            };

            return userProjectDto;
        }

        private UserProject MapCreateDtoToUserProject(UserProjectCreateDto createDto)
        {
            // Implement the mapping from UserProjectCreateDto to UserProject entity
            // Here's an example:
            var userProject = new UserProject
            {
                UserId = createDto.UserId,
                ProjectId = createDto.ProjectId,
                Role = createDto.Role
            };

            return userProject;
        }
    }

}
