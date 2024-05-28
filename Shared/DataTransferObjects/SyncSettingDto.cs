using OpenGitSync.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class SyncSettingDto : IEditable
    {
        [NonEditable]
        public long Id { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }

        public SyncStartTypes SyncStartType { get; set; }
        [NonEditable]
        public ScheduleDto Schedule { get; set; }
        public SyncWays SyncWay { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [NonEditable]
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }

        public long SourceRepositoryId { get; set; }
        public long TargetRepositoryId { get; set; }

        public SyncSettingDto()
        {
            Name = string.Empty;
            Schedule = new ScheduleDto();
        }

        #region IEditable

        private SyncSettingDto? _backupData;
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

            this.Schedule.BeginEdit();
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

            this.Schedule.CancelEdit();

            _backupData = null;
        }

        public void EndEdit()
        {
            this.Schedule.EndEdit();

            _backupData = null;
        }

        #endregion

    }
}
