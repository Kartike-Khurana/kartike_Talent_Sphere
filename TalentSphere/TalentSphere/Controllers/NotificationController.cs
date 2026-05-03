using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs.Notification;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service) => _service = service;

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDTO>>> GetUserNotifications(int userId)
        {
            var data = await _service.GetUserNotificationsAsync(userId);
            return Ok(data);
        }

        [HttpPatch("{id:int}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var result = await _service.MarkAsReadAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Notification {id} not found." });
        }

        [HttpPatch("user/{userId:int}/read-all")]
        public async Task<IActionResult> MarkAllRead(int userId)
        {
            await _service.MarkAllAsReadAsync(userId);
            return NoContent();
        }
    }
}
