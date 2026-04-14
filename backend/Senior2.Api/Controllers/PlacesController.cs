using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.DTOs;
using Senior2.Api.Models;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlacesController(AppDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlaces(string? category, int? activityType)
        {
            var query = _context.Places
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Slug == category);
            }

            if (activityType.HasValue)
            {
                query = query.Where(p => p.ActivityTypeId == activityType.Value);
            }

            var places = await query
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Location,
                    p.ImageUrl,
                    Category = p.Category.Name,
                    Slug = p.Category.Slug
                })
                .ToListAsync();

            return Ok(places);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlace(
     [FromForm] PlaceDto dto,
     IFormFile image)
        {
            string imageUrl = null;

            // 🔥 Save image
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = $"/images/{fileName}";
            }

            var place = new Place
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                Price = dto.Price ?? 0,
                CategoryId = dto.CategoryId,
                ActivityTypeId = dto.ActivityTypeId,
                ImageUrl = imageUrl
            };

            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            return Ok(place);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
                return NotFound("Place not found");

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }
    }
}
