using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Role Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<UserProject> UserProjects { get; set; }
    }

}
