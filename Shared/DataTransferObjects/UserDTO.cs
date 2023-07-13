using DB.Models.Enums;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Role Role { get; set; }
        // Add other properties as needed
    }
}
