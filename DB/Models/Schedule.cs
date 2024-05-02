using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public class Schedule
    {
        #region General

        [Key]
        public long Id { get; set; }
        
        #endregion

        #region Settings

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

        #endregion

        #region Relationships

        public virtual SyncSetting SyncSetting { get; set; }

        #endregion
    }
}
