using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models
{
    public class Repository
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        public string Key { get; set; }
        public RepositoryType RepositoryType { get; set; }

        // Relationships
        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
