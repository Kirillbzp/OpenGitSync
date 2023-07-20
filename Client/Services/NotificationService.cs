using OpenGitSync.Shared.DataTransferObjects;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotifications();
        Task MarkNotificationAsRead(long notificationId);
    }

    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationDto>> GetNotifications()
        {
            return await _httpClient.GetFromJsonAsync<List<NotificationDto>>("api/notifications");
        }

        public async Task MarkNotificationAsRead(long notificationId)
        {
            await _httpClient.PostAsync($"api/notifications/{notificationId}/read", null);
        }
    }
}
