using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ProjectDto : IEditable
    {
        [NonEditable]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        // ToDo: Remove this property
        [NonEditable]
        public List<SyncSettingDto> SyncSettings { get; set; }
        [NonEditable]
        public DateTime CreatedAt { get; set; }
        [NonEditable]
        public DateTime UpdatedAt { get; set; }

        public ProjectDto()
        {
            Name = string.Empty;
            Description = string.Empty;
            SyncSettings = new List<SyncSettingDto>();
        }

        #region IEditable

        private ProjectDto? _backupData { get; set; }
        private IEnumerable<PropertyInfo> prop;

        public void BeginEdit()
        {
            if ( prop == null)
            {
                prop = this.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NonEditableAttribute), false));
            }

            _backupData = new ();
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
