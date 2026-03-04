using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // ============================================
        // GET: api/journey/{userId}
        // ============================================
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserJourneys(int userId)
        {
            var journeys = await _context.JourneyEntries
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(journeys);
        }

        // ============================================
        // POST: api/journey
        // ============================================
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateJourney([FromForm] CreateJourneyRequest request)
        {
            string? mediaUrl = null;
            string? mediaType = null;

            if (request.Media != null)
            {
                var uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/journeys");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName =
                    Guid.NewGuid() + Path.GetExtension(request.Media.FileName);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Media.CopyToAsync(stream);
                }

                mediaUrl = $"/uploads/journeys/{uniqueFileName}";
                mediaType = request.Media.ContentType.StartsWith("video")
                    ? "video"
                    : "image";
            }

            var entry = new JourneyEntry
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                MediaUrl = mediaUrl,
                MediaType = mediaType,
                CreatedAt = DateTime.UtcNow
            };

            _context.JourneyEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(entry);
        }

        // ============================================
        // PUT: api/journey/{id}
        // ============================================
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateJourney(
            int id,
            [FromForm] UpdateJourneyRequest request)
        {
            var existing = await _context.JourneyEntries
                .FirstOrDefaultAsync(j => j.Id == id);

            if (existing == null)
                return NotFound("Journey not found");

            if (existing.UserId != request.UserId)
                return Unauthorized("You cannot edit this journey");

            string? mediaUrl = existing.MediaUrl;
            string? mediaType = existing.MediaType;

            // If new media uploaded → replace old
            if (request.Media != null)
            {
                // Delete old file if exists
                if (!string.IsNullOrEmpty(existing.MediaUrl))
                {
                    var oldFilePath = Path.Combine(
                        _environment.WebRootPath,
                        existing.MediaUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/journeys");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName =
                    Guid.NewGuid() + Path.GetExtension(request.Media.FileName);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Media.CopyToAsync(stream);
                }

                mediaUrl = $"/uploads/journeys/{uniqueFileName}";
                mediaType = request.Media.ContentType.StartsWith("video")
                    ? "video"
                    : "image";
            }

            existing.Title = request.Title;
            existing.Content = request.Content;
            existing.MediaUrl = mediaUrl;
            existing.MediaType = mediaType;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }
    }
}