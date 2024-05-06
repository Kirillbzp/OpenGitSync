using OpenGitSync.Shared.Enums;
using System.Reflection;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ScheduleDto : IEditable
    {
        [NonEditable]
        public long Id { get; set; }
        
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
        
        [NonEditable]
        public long SyncSettingId { get; set; }

        #region IEditable

        private ScheduleDto? _backupData;
        private IEnumerable<PropertyInfo> prop;

        public void BeginEdit()
        {
            if (prop == null)
            {
                prop = this.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NonEditableAttribute), false));
            }

            _backupData = new();
            foreach (var p in prop)
            {
                p.SetValue(_backupData, p.GetValue(this));
            }

        }

        public void CancelEdit()
        {
            if (_backupData == null)
            {
                return;
            }

            foreach (var p in prop)
            {
                p.SetValue(this, p.GetValue(_backupData));
            }

            _backupData = null;
        }

        public void EndEdit()
        {
            _backupData = null;
        }

        #endregion
    }
}
