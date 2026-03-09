using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using System.Text.Json;

namespace Senior2.Api.Services
{
    public class RecommendationService
    {
        private readonly AppDbContext _context;
        private readonly GeoapifyService _geo;

        public RecommendationService(AppDbContext context, GeoapifyService geo)
        {
            _context = context;
            _geo = geo;
        }

        public async Task<List<object>> GetRecommendations(int userId)
        {
            var result = new List<object>();

            // 1️⃣ Get user preferences
            var pref = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            List<string> preferences = new();

            if (pref != null)
            {
                preferences = JsonSerializer.Deserialize<List<string>>(pref.PreferencesJson) ?? new();
            }

            // 2️⃣ Popular places from your platform database
            var dbPlaces = await _context.Places
                .Include(p => p.Category)
                .Take(10)
                .ToListAsync();
            result.Add(new
            {
                title = "Popular on Platform",
                places = dbPlaces.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    imageUrl = string.IsNullOrEmpty(p.ImageUrl)
                        ? "/images/default-place.jpg"
                        : p.ImageUrl,
                    city = p.Location
                })
            });

            // 3️⃣ External recommendations from Geoapify
            var externalPlaces = await _geo.GetLebanonPlaces();

            result.Add(new
            {
                title = "Explore Lebanon",
                places = externalPlaces
            });

            // 4️⃣ Preference-based recommendations
            if (preferences.Any())
            {
                var prefQuery = preferences.First();

                var prefPlaces = await _geo.GetLebanonPlaces();

                result.Add(new
                {
                    title = $"Because you like {prefQuery}",
                    places = prefPlaces
                });
            }

            return result;
        }
    }
}