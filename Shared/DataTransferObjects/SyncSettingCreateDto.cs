using OpenGitSync.Shared.Enums;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class SyncSettingCreateDto
    {
        public SyncSettingCreateDto() 
        {
            Name = string.Empty;
            SyncStartType = SyncStartTypes.Manual;
            SyncWay = SyncWays.ToB;
        }
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public long SourceRepositoryId { get; set; }
        public long TargetRepositoryId { get; set; }
        public SyncStartTypes SyncStartType { get; set; }
        public SyncWays SyncWay { get; set; }
    }

}
