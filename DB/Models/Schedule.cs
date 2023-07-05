using DB.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public IntervalType IntervalType { get; set; }
        public long IntervalCount { get; set; }
        public bool OnSunday { get; set; }
        public bool OnMonday { get; set; }
        public bool OnTuesday { get; set; }
        public bool OnWednesday { get; set; }
        public bool OnThursday { get; set; }
        public bool OnFriday { get; set; }
        public bool OnSaturday { get; set; }
        public long MinimumDeleay { get; set; }
        public long RepeatTaskInterval { get; set; }

        public long SyncSettingId { get; set; }
        [ForeignKey("SyncSettingId")]
        public SyncSetting SyncSetting { get; set; }

    }
}
