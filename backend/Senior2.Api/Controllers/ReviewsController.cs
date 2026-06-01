using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.DTOs;
using Senior2.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{placeId}")]
        public async Task<IActionResult> GetReviews(int placeId)
        {
            var reviews = await _context.PlaceReviews
                .Include(r => r.User)
                .Where(r => r.PlaceId == placeId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    user = r.User.FirstName,
                    r.CreatedAt
                })
                .ToListAsync();

            return Ok(reviews);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            var review = new PlaceReview
            {
                PlaceId = dto.PlaceId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            _context.PlaceReviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _context.PlaceReviews
                .Include(r => r.User)
                .Include(r => r.Place)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    id = r.Id,
                    user = r.User.FirstName + " " + r.User.LastName,
                    place = r.Place.Name,
                    rating = r.Rating,
                    comment = r.Comment,
                    createdAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.PlaceReviews.FindAsync(id);

            if (review == null)
                return NotFound("Review not found");

            _context.PlaceReviews.Remove(review);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Review deleted successfully" });
        }


    }
}