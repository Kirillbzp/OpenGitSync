using DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenGitSync.Server.Models;
using OpenGitSync.Shared.DataTransferObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenGitSync.Server.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto registrationDto);
        Task<LoginResult> LoginAsync(UserLoginDto loginDto);
        // Add other user-related methods as needed: update profile, change password, etc.
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            var user = new User
            {
                UserName = registrationDto.UserName,
                Email = registrationDto.Email
            };

            var result = await _userManager.CreateAsync(user, registrationDto.Password);

            return result;
        }

        public async Task<LoginResult> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return LoginResult.Failed("Invalid email or password.");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return LoginResult.Failed("Invalid email or password.");
            }

            var token = GenerateToken(user);

            return LoginResult.Success(token);
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
            // Add additional claims as needed
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:TokenExpirationDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
