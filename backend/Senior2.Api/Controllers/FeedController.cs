using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Senior2.Api.Data;
using Microsoft.AspNetCore.Authorization;
namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔐 protect all endpoints
    public class FeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedController(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // GET: api/feed
        // 👉 All shared content (stories + journeys)
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetFeed()
        {
            var userId = GetUserId();

            var feed = await _context.JourneyEntries
                .Where(j => j.IsShared && j.UserId != userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(feed);
        }

        // ============================================
        // GET: api/feed/stories
        // 👉 Only stories (video/image quick posts)
        // ============================================
        [HttpGet("stories")]
        public async Task<IActionResult> GetStories()
        {
            var userId = GetUserId();

            var stories = await _context.JourneyEntries
                .Where(j =>
                    j.IsShared &&
                    j.Type == "story" &&
                    j.UserId != userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(stories);
        }

        // ============================================
        // GET: api/feed/journeys
        // 👉 Only journeys (long posts)
        // ============================================
        [HttpGet("journeys")]
        public async Task<IActionResult> GetJourneys()
        {
            var userId = GetUserId();

            var journeys = await _context.JourneyEntries
                .Where(j =>
                    j.IsShared &&
                    j.Type == "journey" &&
                    j.UserId != userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(journeys);
        }

        // ============================================
        // 🔒 Helper: Get logged-in user ID safely
        // ============================================
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return int.Parse(userIdClaim.Value);
        }
    }
}