using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models
{
    public class Repository
    {
        public Repository() 
        { 
            Key = string.Empty;
            RepositoryType = RepositoryType.Git;
        }

        #region General

        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        public string Key { get; set; }
        public RepositoryType RepositoryType { get; set; }

        #endregion

        #region Relationships

        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #endregion
    }
}
