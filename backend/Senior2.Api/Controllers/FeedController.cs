using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using System.Security.Claims;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetFeed()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var feed = await _context.JourneyEntries
                .Where(j => j.IsShared && j.UserId != userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(feed);
        }
    }
}