using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Senior2.Api.Controllers
{
    [Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ReportService _reportService;
        private readonly EmailService _emailService;


        public AdminController(AppDbContext context, ReportService reportService, EmailService emailService)
        {
            _context = context;
            _reportService = reportService;
            _emailService = emailService;
        }

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

            var totalReviews = await _context.PlaceReviews.CountAsync();

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

            var trips = await _context.SmartItineraryRequest
                .Where(t => t.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new
                {
                    date = g.Key,
                    count = g.Count()
                })
                .ToListAsync();


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


        [HttpGet("report/pdf")]
        public async Task<IActionResult> GenerateReportPdf()
        {
            var users = await _context.Users.CountAsync();
            var trips = await _context.Set<SmartItineraryRequest>().CountAsync();
            var reviews = await _context.PlaceReviews.CountAsync();
            var suggestions = await _context.Suggestions.CountAsync();

            var pdfBytes = _reportService.GenerateReportPdf(
                users,
                trips,
                reviews,
                suggestions
            );

            return File(pdfBytes, "application/pdf", "platform-report.pdf");
        }
        [HttpPost("report/send-warnings")]
        public async Task<IActionResult> SendWarningEmails()
        {
            var warnedUsers = await _context.Users
                .Where(u => u.IsBlocked)
                .ToListAsync();

            foreach (var user in warnedUsers)
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Warning considering your behavior on the website",
                    $@"
            <h2>Hello {user.FirstName},</h2>
            <p>Your account has been flagged for unethical behavior on the platform.</p>
            <p>Please respect the platform rules to avoid permanent restriction.</p>
            "
                );
            }

            return Ok(new
            {
                message = $"Warning emails sent to {warnedUsers.Count} users."
            });
        }


    }
}