using OpenGitSync.Shared.Enums;
using System.ComponentModel;
using System.Reflection;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class RepositoryDto : IEditable
    {
        [NonEditable]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public RepositoryType RepositoryType { get; set; }
        [NonEditable]
        public DateTime CreatedAt { get; set; }
        [NonEditable]
        public DateTime UpdatedAt { get; set; }
        [NonEditable]
        public long ProjectId { get; set; }

        public RepositoryDto()
        {
            Name = string.Empty;
            Url = string.Empty;
            Token = string.Empty;
        }

        #region IEditable

        private RepositoryDto? _backupData;
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
