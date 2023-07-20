using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;

namespace OpenGitSync.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ApiControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(IApiControllerWrapper wrapper, INotificationService notificationService) : base(wrapper)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var notifications = _notificationService.GetNotificationsByUserId(UserId);
            return Ok(notifications);
        }

        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(long id)
        {
            var result = await _notificationService.MarkNotificationAsRead(UserId, id);
            if (result)
            {
                return Ok();
            }
            return NotFound(new { Error = "Notification not found" });
        }

        // Other actions as needed: Create, Delete, etc.
    }
}
