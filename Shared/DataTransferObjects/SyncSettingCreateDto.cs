namespace OpenGitSync.Shared.DataTransferObjects
{
    public class SyncSettingCreateDto
    {
        public long ProjectId { get; set; }
        public long SourceRepositoryId { get; set; }
        public long TargetRepositoryId { get; set; }
        public TimeSpan Frequency { get; set; }
    }

}
