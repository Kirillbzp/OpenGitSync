using OpenGitSync.Shared.Enums;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Role Role { get; set; }
        // Add other properties as needed
    }
}
