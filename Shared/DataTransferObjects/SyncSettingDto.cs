using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class SyncSettingDto
    {
        public long Id { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }

        public SyncStartTypes SyncStartType { get; set; }
        public ScheduleDto Schedule { get; set; }
        public SyncWays SyncWay { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        
        public long ProjectId { get; set; }
        
        public long SourceRepositoryId { get; set; }
        public long TargetRepositoryId { get; set; }
        
    }
}
