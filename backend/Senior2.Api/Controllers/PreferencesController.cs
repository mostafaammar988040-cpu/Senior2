using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Text.Json;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/preferences")]
    public class PreferencesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PreferencesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SavePreferences([FromBody] PreferenceRequest req)
        {
            var json = JsonSerializer.Serialize(req.Preferences);

            var existing = _context.UserPreferences
                .FirstOrDefault(p => p.UserId == req.UserId);

            if (existing != null)
            {
                existing.PreferencesJson = json;
            }
            else
            {
                var pref = new UserPreference
                {
                    UserId = req.UserId,
                    PreferencesJson = json
                };

                _context.UserPreferences.Add(pref);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    public class PreferenceRequest
    {
        public int UserId { get; set; }
        public object Preferences { get; set; }
    }
}