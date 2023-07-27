using OpenGitSync.Server.Models;

namespace DB.Helpers
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNotificationsByUserId(string userId);
        Notification GetNotificationById(long notificationId);
        void UpdateNotification(Notification notification);
        // Other methods as needed: CreateNotification, DeleteNotification, etc.
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Notification> GetNotificationsByUserId(string userId)
        {
            return _dbContext.Notifications.Where(n => n.UserId == userId);
        }

        public Notification GetNotificationById(long notificationId)
        {
            return _dbContext.Notifications.FirstOrDefault(n => n.Id == notificationId);
        }

        public void UpdateNotification(Notification notification)
        {
            _dbContext.Notifications.Update(notification);
            _dbContext.SaveChanges();
        }

        // Implement other methods as needed: CreateNotification, DeleteNotification, etc.
    }
}
