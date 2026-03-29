using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.DTOS;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JourneyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public JourneyController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ================================
        // GET USER JOURNEYS
        // ================================
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserJourneys(int userId)
        {
            var journeys = await _context.JourneyEntries
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(journeys);
        }

        // ================================
        // CREATE JOURNEY
        // ================================
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateJourney([FromForm] CreateJourneyRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string? mediaUrl = null;
            string? mediaType = null;

            if (request.Media != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/journeys");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.Media.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Media.CopyToAsync(stream);
                }

                // 🔥 FIX: FULL URL
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                mediaUrl = $"{baseUrl}/uploads/journeys/{fileName}";

                mediaType = request.Media.ContentType.StartsWith("video")
                    ? "video"
                    : "image";
            }

            var entry = new JourneyEntry
            {
                UserId = userId,
                Title = request.Title,
                Content = request.Content,
                MediaUrl = mediaUrl,
                MediaType = mediaType,
                CreatedAt = DateTime.UtcNow,
                IsShared = request.IsShared,
                Type = "journey"
            };

            _context.JourneyEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(entry);
        }

        // ================================
        // UPDATE JOURNEY
        // ================================
        [HttpPut("{id}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateJourney(int id, [FromForm] UpdateJourneyRequest request)
        {
            var existing = await _context.JourneyEntries.FirstOrDefaultAsync(j => j.Id == id);

            if (existing == null)
                return NotFound("Journey not found");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (existing.UserId != userId)
                return Unauthorized("You cannot edit this journey");

            string? mediaUrl = existing.MediaUrl;
            string? mediaType = existing.MediaType;

            if (request.Media != null)
            {
                // delete old file
                if (!string.IsNullOrEmpty(existing.MediaUrl))
                {
                    var oldPath = existing.MediaUrl.Replace($"{Request.Scheme}://{Request.Host}", "");
                    var oldFilePath = Path.Combine(_environment.WebRootPath, oldPath.TrimStart('/'));

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/journeys");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.Media.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Media.CopyToAsync(stream);
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                mediaUrl = $"{baseUrl}/uploads/journeys/{fileName}";

                mediaType = request.Media.ContentType.StartsWith("video")
                    ? "video"
                    : "image";
            }

            existing.Title = request.Title;
            existing.Content = request.Content;
            existing.MediaUrl = mediaUrl;
            existing.MediaType = mediaType;
            existing.IsShared = request.IsShared;
            existing.Type = "journey";

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // ================================
        // CREATE STORY
        // ================================
        [HttpPost("story")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateStory([FromForm] CreateStoryRequest request)
        {
            if (request.Media == null)
                return BadRequest("Media is required");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/journeys");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(request.Media.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Media.CopyToAsync(stream);
            }

            // 🔥 FIX: FULL URL
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var mediaUrl = $"{baseUrl}/uploads/journeys/{fileName}";

            var mediaType = request.Media.ContentType.StartsWith("video")
                ? "video"
                : "image";

            var entry = new JourneyEntry
            {
                UserId = userId,
                MediaUrl = mediaUrl,
                MediaType = mediaType,
                CreatedAt = DateTime.UtcNow,
                Type = "story",
                IsShared = true
            };

            _context.JourneyEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(entry);
        }
    }
}