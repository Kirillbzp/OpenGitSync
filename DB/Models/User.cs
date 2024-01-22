using DB.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace DB.Models
{
    public class User : IdentityUser
    {
        public Role Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<UserProject> UserProjects { get; set; }
    }

}
