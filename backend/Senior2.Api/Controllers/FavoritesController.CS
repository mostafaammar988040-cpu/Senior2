using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Microsoft.AspNetCore.Authorization;
namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================
        // ADD FAVORITE
        // ==========================
        [HttpPost("add")]
        public async Task<IActionResult> AddFavorite(int userId, int placeId)
        {
            var exists = _context.Favorites
                .Any(f => f.UserId == userId && f.PlaceId == placeId);

            if (exists)
                return BadRequest("Already added");

            var fav = new Favorite
            {
                UserId = userId,
                PlaceId = placeId
            };

            _context.Favorites.Add(fav);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ==========================
        // REMOVE FAVORITE
        // ==========================
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFavorite(int userId, int placeId)
        {
            var fav = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.PlaceId == placeId);

            if (fav == null)
                return NotFound();

            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ==========================
        // GET USER FAVORITES
        // ==========================
        [HttpGet("{userId}")]
        public IActionResult GetFavorites(int userId)
        {
            var favs = _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Place)
                .ToList();

            return Ok(favs);
        }
    }
}