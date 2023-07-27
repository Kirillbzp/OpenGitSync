using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Helpers;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserController(IApiControllerWrapper wrapper, IUserService userService, UserManager<User> userManager) : base(wrapper)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            // Retrieve the user from the service
            var user = _userService.GetUserById(UserId);

            if (user == null)
                return NotFound(new { Error = "User not found" });

            // Map the user to the UserProfileDto
            var userProfile = new UserProfileDto
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return Ok(userProfile);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var result = await _userService.RegisterUserAsync(registrationDto);

            if (result.Succeeded)
            {
                return Ok(new ResponceResultDto());
            }

            return BadRequest(result.ToError());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(loginDto);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile(UserProfileDto profileDto)
        {
            // Retrieve the user from the database
            var user = _userService.GetUserById(UserId);

            if (user == null)
                return NotFound(new { Error = "User not found" });

            // Update the user's profile information
            user.UserName = profileDto.UserName;
            user.Email = profileDto.Email;

            // Save the changes to the database
            _userService.UpdateUser(user);

            // Return the updated user profile
            return Ok(profileDto);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
        {
            // Retrieve the user from the database
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return NotFound(new { Error = "User not found" });

            // Validate the current password
            var passwordValid = await _userManager.CheckPasswordAsync(user, passwordDto.CurrentPassword);
            if (!passwordValid)
                return BadRequest(new { Error = "Invalid current password" });

            // Change the user's password
            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new { Error = "Failed to change password" });

            return Ok(new { Message = "Password changed successfully" });
        }

        [HttpGet("{userId}/projects")]
        public IActionResult GetUserProjects(string userId)
        {
            // Retrieve the user projects from the service
            var projects = _userService.GetUserProjects(userId);

            if (projects == null)
                return NotFound(new { Error = "User not found" });

            // Return the user projects
            return Ok(projects);
        }

    }

}
