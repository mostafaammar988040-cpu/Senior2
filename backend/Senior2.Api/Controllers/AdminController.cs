using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    id = u.Id,
                    name = u.FirstName + " " + u.LastName,
                    email = u.Email,
                    createdAt = u.CreatedAt,
                    blocked = u.IsBlocked
                })
                .ToListAsync();

            return Ok(users);
        }
        [HttpPut("block-user/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.IsBlocked = true;

            _context.Notifications.Add(new Notification
            {
                UserId = user.Id,
                Message = "Your account has been blocked by the administrator."
            });

            await _context.SaveChangesAsync();

            return Ok("User blocked");
        }

        [HttpPut("unblock-user/{id}")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.IsBlocked = false;
            _context.Notifications.Add(new Notification
            {
                UserId = user.Id,
                Message = "Your account has been unblocked. You can now login again."
            });
            await _context.SaveChangesAsync();

            return Ok("User unblocked");
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalUsers = await _context.Users.CountAsync();

            var totalTrips = await _context.SmartItineraryRequest.CountAsync();

            // if you have reviews table
            var totalReviews = await _context.PlaceReviews.CountAsync();

            // example for reports/flags if you have them
            var totalFlags = await _context.SupportRequests.CountAsync();

            return Ok(new
            {
                users = totalUsers,
                trips = totalTrips,
                reviews = totalReviews,
                flags = totalFlags
            });
        }
        [HttpGet("dashboard-charts")]
        public async Task<IActionResult> GetDashboardCharts()
        {
            // Trips per day (last 7 days)

            var trips = await _context.SmartItineraryRequest
                .Where(t => t.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new
                {
                    date = g.Key,
                    count = g.Count()
                })
                .ToListAsync();

            // Reviews rating distribution

            var ratings = await _context.PlaceReviews
                .GroupBy(r => r.Rating)
                .Select(g => new
                {
                    rating = g.Key,
                    count = g.Count()
                })
                .ToListAsync();

            return Ok(new
            {
                tripsPerDay = trips,
                ratings = ratings
            });
        }
    }
}