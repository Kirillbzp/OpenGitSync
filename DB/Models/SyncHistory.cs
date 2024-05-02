using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    internal class SyncHistory
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public SyncSetting Setting { get; set; }
        public HistoryType HistoryType { get; set; }
        public string ErrorDescription { get; set; }
    }
}
