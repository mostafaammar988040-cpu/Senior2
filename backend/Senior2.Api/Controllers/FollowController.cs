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
    public class FollowController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FollowController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/follow
        [HttpPost("{followedId}")]
        public async Task<IActionResult> FollowUser(int followedId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (userId == followedId)
                return BadRequest("You cannot follow yourself.");

            var already = await _context.Follows
                .AnyAsync(f => f.FollowerId == userId && f.FollowedId == followedId);

            if (already)
                return BadRequest("Already following.");

            var follow = new Follow
            {
                FollowerId = userId,
                FollowedId = followedId
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{followedId}")]
        public async Task<IActionResult> UnfollowUser(int followedId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == userId && f.FollowedId == followedId);

            if (follow == null)
                return NotFound();

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}