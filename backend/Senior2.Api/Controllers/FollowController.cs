using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FollowController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{followedId}")]
        public async Task<IActionResult> FollowUser(int followedId)
        {
            var userId = GetUserId();

            if (userId == followedId)
                return BadRequest("You cannot follow yourself.");

            var userExists = await _context.Users.AnyAsync(u => u.Id == followedId);

            if (!userExists)
                return NotFound("User not found.");

            var already = await _context.Follows
                .AnyAsync(f => f.FollowerId == userId && f.FollowedId == followedId);

            if (already)
            {
                return Ok(new
                {
                    isFollowing = true,
                    message = "Already following."
                });
            }

            var follow = new Follow
            {
                FollowerId = userId,
                FollowedId = followedId
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                isFollowing = true,
                message = "Followed successfully."
            });
        }

        [HttpDelete("{followedId}")]
        public async Task<IActionResult> UnfollowUser(int followedId)
        {
            var userId = GetUserId();

            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == userId && f.FollowedId == followedId);

            if (follow == null)
            {
                return Ok(new
                {
                    isFollowing = false,
                    message = "You are not following this user."
                });
            }

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                isFollowing = false,
                message = "Unfollowed successfully."
            });
        }

        [HttpGet("status/{followedId}")]
        public async Task<IActionResult> GetFollowStatus(int followedId)
        {
            var userId = GetUserId();

            var isFollowing = await _context.Follows
                .AnyAsync(f => f.FollowerId == userId && f.FollowedId == followedId);

            return Ok(new
            {
                isFollowing
            });
        }

        [HttpGet("followers")]
        public async Task<IActionResult> GetMyFollowers()
        {
            var userId = GetUserId();

            var followers = await _context.Follows
                .Where(f => f.FollowedId == userId)
                .Join(
                    _context.Users,
                    follow => follow.FollowerId,
                    user => user.Id,
                    (follow, user) => new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        profileImageUrl = user.ProfileImageUrl
                    }
                )
                .ToListAsync();

            return Ok(followers);
        }

        [HttpGet("following")]
        public async Task<IActionResult> GetMyFollowing()
        {
            var userId = GetUserId();

            var following = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Join(
                    _context.Users,
                    follow => follow.FollowedId,
                    user => user.Id,
                    (follow, user) => new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        profileImageUrl = user.ProfileImageUrl
                    }
                )
                .ToListAsync();

            return Ok(following);
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}