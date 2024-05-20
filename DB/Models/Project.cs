using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public class Project
    {
        #region General

        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #endregion

        #region Relationships

        public List<RepositoryModel> Repositories { get; set; }
        public List<UserProject> UserProjects { get; set; }
        public List<SyncSetting> SyncSettings{ get; set; }

        #endregion
    }

}
