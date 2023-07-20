namespace OpenGitSync.Shared.DataTransferObjects
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
