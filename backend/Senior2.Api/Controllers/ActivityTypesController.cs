using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActivityTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityTypes()
        {
            var types = await _context.ActivityTypes.ToListAsync();
            return Ok(types);
        }

        [HttpGet("by-category")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var types = await _context.ActivityTypes
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();

            return Ok(types);
        }
    }
}