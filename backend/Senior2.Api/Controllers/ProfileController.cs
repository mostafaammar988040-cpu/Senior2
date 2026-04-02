using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        // ======================================
        // UPDATE LOGGED-IN USER PROFILE
        // ======================================
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            // Update fields
            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email ?? user.Email;
            user.Bio = dto.Bio ?? user.Bio;
            user.ProfileImageUrl = dto.ProfileImageUrl ?? user.ProfileImageUrl;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Profile updated successfully",
                user = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.ProfileImageUrl,
                    user.Bio,
                    user.CreatedAt
                }
            });
        }
        // ======================================
        // CHANGE PASSWORD (LOGGED-IN USER)
        // ======================================
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            // Verify current password
            bool valid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
            if (!valid) return BadRequest("Current password is incorrect");

            // Hash and update new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully" });
        }

    }
}