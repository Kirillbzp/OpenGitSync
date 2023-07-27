using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserProjectController(IApiControllerWrapper wrapper, IUserProjectService userProjectService, IMapper mapper) : base(wrapper)
        {
            _userProjectService = userProjectService;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<UserProjectDto> AddUserToProject(UserProjectCreateDto createDto)
        {
            var userProject = _mapper.Map<UserProject>(createDto);

            _userProjectService.AddUserToProject(userProject);

            var userProjectDto =  _mapper.Map<UserProjectDto>(userProject);
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

            var userProjectDto = _mapper.Map<UserProjectDto>(userProject);
            return Ok(userProjectDto);
        }

        [HttpGet("project/{projectId}")]
        public ActionResult<IEnumerable<UserProjectDto>> GetUserProjectsByProjectId(long projectId)
        {
            var userProjects = _userProjectService.GetUserProjectsByProjectId(projectId);

            var userProjectDtos = userProjects.Select(up => _mapper.Map<UserProjectDto>(up));
            return Ok(userProjectDtos);
        }

    }

}
