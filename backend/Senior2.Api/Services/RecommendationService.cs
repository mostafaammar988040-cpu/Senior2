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

            // 2️⃣ Popular places from your platform
            var dbPlaces = await _context.Places
                .Include(p => p.Category)
                .Take(8)
                .ToListAsync();

            result.Add(new
            {
                title = "Popular on Platform",
                places = dbPlaces.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    imageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/default-place.jpg" : p.ImageUrl,
                    city = p.Location
                })
            });

            // 3️⃣ Restaurants from database
            var restaurants = await _context.Places
                .Include(p => p.Category)
                .Where(p => p.Category.Slug == "restaurants")
                .Take(8)
                .ToListAsync();

            result.Add(new
            {
                title = "Best Restaurants For You",
                places = restaurants.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    imageUrl = p.ImageUrl,
                    city = p.Location
                })
            });

            // 4️⃣ Personalized recommendations
            if (pref != null)
            {
                var prefs = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(pref.PreferencesJson);

                if (prefs != null && prefs.ContainsKey("activities"))
                {
                    var activities = prefs["activities"];

                    // 🥾 Hiking
                    if (activities.Any(a => a.Contains("hike")))
                    {
                        var hiking = await _context.Places
                            .Include(p => p.ActivityType)
                            .Where(p => p.ActivityType != null && p.ActivityType.Slug == "hiking")
                            .Take(6)
                            .ToListAsync();

                        result.Add(new
                        {
                            title = "Hiking Adventures",
                            places = hiking.Select(p => new
                            {
                                id = p.Id,
                                name = p.Name,
                                imageUrl = p.ImageUrl,
                                city = p.Location
                            })
                        });
                    }

                    // 🏖 Beaches / Swimming
                    if (activities.Any(a => a.Contains("beach")))
                    {
                        var swimming = await _context.Places
                            .Include(p => p.ActivityType)
                            .Where(p => p.ActivityType != null && p.ActivityType.Slug == "swimming")
                            .Take(6)
                            .ToListAsync();

                        result.Add(new
                        {
                            title = "Best Swimming Spots",
                            places = swimming.Select(p => new
                            {
                                id = p.Id,
                                name = p.Name,
                                imageUrl = p.ImageUrl,
                                city = p.Location
                            })
                        });
                    }

                    // 🎿 Skiing
                    if (activities.Any(a => a.Contains("ski")))
                    {
                        var skiing = await _context.Places
                            .Include(p => p.ActivityType)
                            .Where(p => p.ActivityType != null && p.ActivityType.Slug == "skiing")
                            .Take(6)
                            .ToListAsync();

                        result.Add(new
                        {
                            title = "Skiing Adventures",
                            places = skiing.Select(p => new
                            {
                                id = p.Id,
                                name = p.Name,
                                imageUrl = p.ImageUrl,
                                city = p.Location
                            })
                        });
                    }
                }
            }

            // 5️⃣ Explore Lebanon (external discovery)
            var explore = await _geo.GetLebanonPlaces("tourism.sights");

            result.Add(new
            {
                title = "Explore Lebanon",
                places = explore
            });

            return result;
        }
    }
}