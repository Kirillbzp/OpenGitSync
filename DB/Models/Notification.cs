using DB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGitSync.Server.Models
{
    public class Notification
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
