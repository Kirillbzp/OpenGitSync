using DB.Models.Enums;

namespace DB.Models
{
    internal class SyncHistory
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public SyncSetting Setting { get; set; }
        public HistoryType HistoryType { get; set; }
        public string ErrorDescription { get; set; }
    }
}
