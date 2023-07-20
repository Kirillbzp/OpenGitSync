namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ProjectDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SyncSettingDto> SyncSettings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
