using DB.Helpers;
using DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenGitSync.Server.Models;
using OpenGitSync.Shared.DataTransferObjects;
using OpenGitSync.Shared.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenGitSync.Server.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto registrationDto);
        Task<LoginResult> LoginAsync(UserLoginDto loginDto);
        Task<UserDto> GetUserById(string userId);
        Task UpdateUser(UserDto userDto);
        Task<bool> ValidateUserPassword(UserDto userDto, string password);
        string HashPassword(string password);
        Task<IEnumerable<ProjectDto>> GetUserProjects(string userId);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly JwtBearerTokenSettings _tokenSettings;

        public UserService(UserManager<User> userManager, 
                           SignInManager<User> signInManager, 
                           IConfiguration configuration, 
                           IUserRepository userRepository, 
                           IProjectRepository projectRepository,
                           IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _tokenSettings = jwtTokenOptions.Value;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            var user = new User
            {
                UserName = registrationDto.UserName,
                Email = registrationDto.Email,
                RegistrationDate = DateTime.UtcNow,
                Role = DB.Models.Enums.Role.User
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

            await _signInManager.SignInAsync(user, true);

            return LoginResult.Success(token);
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
                // ToDo: Add additional claims
            };

            //var tokenSettings = _configuration.GetSection("Jwt").Get<JwtBearerTokenSettings>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_tokenSettings.ExpiryTimeInHours));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = creds,
                Audience = _tokenSettings.Audience,
                Issuer = _tokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        
        public async Task<UserDto> GetUserById(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                return null;

            // Map the User entity to UserDto
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                // Map other properties as needed
            };

            return userDto;
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var user = await _userRepository.GetUserById(userDto.Id);
            if (user == null)
                return;

            // Update the user properties from the userDto
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            // Update other properties as needed

            await _userRepository.UpdateUser(user);
        }

        public async Task<bool> ValidateUserPassword(UserDto userDto, string password)
        {
            // Retrieve the user entity by the userDto.Id
            var user = await _userRepository.GetUserById(userDto.Id);
            if (user == null)
                return false;

            // Compare the provided password with the stored password
            // Return true if they match, false otherwise
            return user.PasswordHash == HashPassword(password);
        }

        public string HashPassword(string password)
        {
            // Implement password hashing logic using a secure hashing algorithm
            // Here's an example using the BCrypt hashing algorithm:
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public async Task<IEnumerable<ProjectDto>> GetUserProjects(string userId)
        {
            var projects = await _projectRepository.GetProjects(userId);
            if (projects == null)
                return null;

            // Map the Project entities to ProjectDto list
            var projectDtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var projectDto = new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    // Map other properties as needed
                };
                projectDtos.Add(projectDto);
            }

            return projectDtos;
        }
    }
}
