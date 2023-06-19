using DB.Models;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class SyncSettingUpdateDto
    {
        public long SourceRepositoryId { get; set; }
        public long TargetRepositoryId { get; set; }
        public ScheduleDto Schedule { get; set; }
    }

}
