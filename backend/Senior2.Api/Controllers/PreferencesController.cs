using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

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
            if (req == null || req.Preferences == null)
                return BadRequest("Invalid preferences");

            var json = JsonSerializer.Serialize(req.Preferences);

            var existing = _context.UserPreferences
                .FirstOrDefault(p => p.UserId == req.UserId);

            if (existing != null)
            {
                existing.PreferencesJson = json;
                existing.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var pref = new UserPreference
                {
                    UserId = req.UserId,
                    PreferencesJson = json,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserPreferences.Add(pref);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Preferences saved successfully" });
        }

       
        [HttpGet("{userId}")]
        public IActionResult GetPreferences(int userId)
        {
            var pref = _context.UserPreferences
                .FirstOrDefault(p => p.UserId == userId);

            if (pref == null)
                return NotFound(new { message = "No preferences found" });

            var parsed = JsonSerializer.Deserialize<PreferencesDto>(pref.PreferencesJson);

            return Ok(parsed);
        }

      
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeletePreferences(int userId)
        {
            var pref = _context.UserPreferences
                .FirstOrDefault(p => p.UserId == userId);

            if (pref == null)
                return NotFound();

            _context.UserPreferences.Remove(pref);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Preferences deleted" });
        }
    }

    
    public class PreferenceRequest
    {
        public int UserId { get; set; }
        public PreferencesDto Preferences { get; set; }
    }


    public class PreferencesDto
    {
        public List<string> Interests { get; set; } = new();
        public List<string> Activities { get; set; } = new();
        public List<string> Food { get; set; } = new();
    }
}