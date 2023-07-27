using DB.Helpers;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface INotificationService
    {
        IEnumerable<NotificationDto> GetNotificationsByUserId(string userId);
        Task<bool> MarkNotificationAsRead(string userId, long notificationId);
        // Other methods as needed: CreateNotification, DeleteNotification, etc.
    }


    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<NotificationDto> GetNotificationsByUserId(string userId)
        {
            var notifications = _notificationRepository.GetNotificationsByUserId(userId);
            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                ReceivedAt = n.ReceivedAt
                // Map other properties as needed
            });
            return notificationDtos;
        }

        public async Task<bool> MarkNotificationAsRead(string userId, long notificationId)
        {
            var notification = _notificationRepository.GetNotificationById(notificationId);
            if (notification != null && notification.UserId == userId)
            {
                notification.IsRead = true;
                _notificationRepository.UpdateNotification(notification);
                return true;
            }
            return false;
        }

        // Implement other methods as needed: CreateNotification, DeleteNotification, etc.
    }
}