using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IApiControllerWrapper wrapper, IUserService userService) : base(wrapper)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(registrationDto);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(loginDto);

            if (result.Succeeded)
            {
                return Ok(new { Token = result.Token });
            }

            return Unauthorized();
        }

        // Add other user-related actions as needed: update profile, change password, etc.
    }

}
