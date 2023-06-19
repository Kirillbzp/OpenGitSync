namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ProjectCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RepositoryA { get; set; }
        public string RepositoryB { get; set; }
        public bool IsSyncEnabled { get; set; }
        public List<SyncSettingDto> SyncFrequency { get; set; }
    }

}
