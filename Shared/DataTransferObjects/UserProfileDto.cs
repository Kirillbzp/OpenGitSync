using System.ComponentModel.DataAnnotations;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class UserProfileDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}