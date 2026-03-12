using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Text;
using System.Linq;

namespace Senior2.Api.Services;

public class PlaceSearchService
{
    private readonly AppDbContext _context;

    public PlaceSearchService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Place>> SearchPlacesAsync(string query)
    {
        var keywords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var queryable = _context.Places
            .Include(p => p.Category)
            .AsQueryable();

        if (keywords.Any())
        {
            queryable = queryable.Where(p =>
                keywords.Any(k =>
                    p.Name.ToLower().Contains(k) ||
                    p.Description.ToLower().Contains(k) ||
                    (p.Category != null && p.Category.Name.ToLower().Contains(k))
                ));
        }

        var places = await queryable
            .Take(5)
            .ToListAsync();

        return places;
    }

    public string FormatPlacesForPrompt(List<Place> places)
    {
        if (!places.Any())
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine("Here are some relevant places from our database:");

        foreach (var place in places)
        {
            sb.AppendLine($"- **{place.Name}** ({place.Category?.Name ?? "Uncategorized"})");
            if (!string.IsNullOrEmpty(place.Description))
                sb.AppendLine($"  Description: {place.Description}");
            if (!string.IsNullOrEmpty(place.Location))
                sb.AppendLine($"  Location: {place.Location}");
        }

        sb.AppendLine("Use this information to help the user.");
        return sb.ToString();
    }
}