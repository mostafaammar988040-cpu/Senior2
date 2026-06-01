using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using System.Security.Claims;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = GetUserId();

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetUserId();

            var count = await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);

            return Ok(new
            {
                count
            });
        }

        [HttpPut("read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = GetUserId();

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notification == null)
                return NotFound();

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetUserId();

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}