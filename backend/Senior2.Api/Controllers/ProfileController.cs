using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using System.Security.Claims;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔥 REQUIRE LOGIN
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        // ======================================
        // GET LOGGED-IN USER PROFILE
        // ======================================
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            // 🔥 GET USER ID FROM JWT TOKEN
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            // ===== USER =====
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            // ===== PREFERENCES =====
            var preferences = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            // ===== TRIPS =====
            var trips = await _context.SmartItineraryRequest
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            // ===== JOURNEYS =====
            var journeys = await _context.JourneyEntries
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(new
            {
                user = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.ProfileImageUrl,
                    user.Bio,
                    user.CreatedAt
                },
                preferences,
                trips,
                journeys
            });
        }
    }
}