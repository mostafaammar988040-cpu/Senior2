using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Senior2.Api.Data;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeed()
        {
            var userId = GetUserId();

            var feed = await _context.JourneyEntries
                .Where(j => j.IsShared && j.UserId != userId)
                .Join(
                    _context.Users,
                    journey => journey.UserId,
                    user => user.Id,
                    (journey, user) => new
                    {
                        journey.Id,
                        journey.UserId,
                        journey.Title,
                        journey.Content,
                        journey.MediaUrl,
                        journey.MediaType,
                        journey.CreatedAt,
                        journey.IsShared,
                        journey.Type,

                        userFirstName = user.FirstName,
                        userLastName = user.LastName,

                        isFollowing = _context.Follows.Any(f =>
                            f.FollowerId == userId &&
                            f.FollowedId == journey.UserId)
                    }
                )
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(feed);
        }

        [HttpGet("stories")]
        public async Task<IActionResult> GetStories()
        {
            var userId = GetUserId();

            var stories = await _context.JourneyEntries
                .Where(j =>
                    j.IsShared &&
                    j.Type == "story" &&
                    j.UserId != userId)
                .Join(
                    _context.Users,
                    story => story.UserId,
                    user => user.Id,
                    (story, user) => new
                    {
                        story.Id,
                        story.UserId,
                        story.Title,
                        story.Content,
                        story.MediaUrl,
                        story.MediaType,
                        story.CreatedAt,
                        story.IsShared,
                        story.Type,

                        userFirstName = user.FirstName,
                        userLastName = user.LastName,

                        isFollowing = _context.Follows.Any(f =>
                            f.FollowerId == userId &&
                            f.FollowedId == story.UserId)
                    }
                )
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(stories);
        }

        [HttpGet("journeys")]
        public async Task<IActionResult> GetJourneys()
        {
            var userId = GetUserId();

            var journeys = await _context.JourneyEntries
                .Where(j =>
                    j.IsShared &&
                    j.Type == "journey" &&
                    j.UserId != userId)
                .Join(
                    _context.Users,
                    journey => journey.UserId,
                    user => user.Id,
                    (journey, user) => new
                    {
                        journey.Id,
                        journey.UserId,
                        journey.Title,
                        journey.Content,
                        journey.MediaUrl,
                        journey.MediaType,
                        journey.CreatedAt,
                        journey.IsShared,
                        journey.Type,

                        userFirstName = user.FirstName,
                        userLastName = user.LastName,

                        isFollowing = _context.Follows.Any(f =>
                            f.FollowerId == userId &&
                            f.FollowedId == journey.UserId)
                    }
                )
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return Ok(journeys);
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return int.Parse(userIdClaim.Value);
        }
    }
}