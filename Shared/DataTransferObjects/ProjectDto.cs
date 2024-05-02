using System.ComponentModel.DataAnnotations;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ProjectDto : IEditable
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SyncSettingDto> SyncSettings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private ProjectDto BackUp { get; set; }

        public void EditMode()
        {
            BackUp = new ProjectDto
            {
                Name = this.Name,
                Description = this.Description,
            };
        }

        public void Restore()
        {
            this.Name = BackUp.Name;
            this.Description = BackUp.Description;
        }
    }

}
