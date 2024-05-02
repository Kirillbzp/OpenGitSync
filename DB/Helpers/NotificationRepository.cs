using Microsoft.EntityFrameworkCore;
using OpenGitSync.Server.Models;

namespace DB.Helpers
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId);
        Task<Notification> GetNotificationById(long notificationId);
        Task UpdateNotification(Notification notification);
        // Other methods as needed: CreateNotification, DeleteNotification, etc.
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId)
        {
            return await _dbContext.Notifications.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<Notification> GetNotificationById(long notificationId)
        {
            return await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
        }

        public async Task UpdateNotification(Notification notification)
        {
            _dbContext.Notifications.Update(notification);
            await _dbContext.SaveChangesAsync();
        }

        // Implement other methods as needed: CreateNotification, DeleteNotification, etc.
    }
}
