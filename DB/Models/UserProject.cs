using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models
{
    public class UserProject
    {
        [Key]
        public long Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public ProjectPermission Role { get; set; }
        
    }

}
