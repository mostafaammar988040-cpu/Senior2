using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Text;

namespace Senior2.Api.Services;

public class PlaceSearchService
{
    private readonly AppDbContext _context;
    private readonly GooglePlacesService _googlePlacesService;

    public PlaceSearchService(AppDbContext context, GooglePlacesService googlePlacesService)
    {
        _context = context;
        _googlePlacesService = googlePlacesService;
    }

    public async Task<List<Place>> SearchPlacesAsync(string query)
    {
        var lowerQuery = query.ToLower();

        // =========================
        // 🔹 1. DETECT CATEGORY (SMART FILTER)
        // =========================
        string categoryFilter = "";

        if (lowerQuery.Contains("coffee") || lowerQuery.Contains("cafe"))
            categoryFilter = "cafe";
        else if (lowerQuery.Contains("restaurant") || lowerQuery.Contains("dinner"))
            query = $"best restaurants in {query} Lebanon";
        else if (lowerQuery.Contains("hotel"))
            categoryFilter = "hotel";

        // =========================
        // 🔹 2. SEARCH LOCAL DB (PRIORITY)
        // =========================
        var dbQuery = _context.Places
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(categoryFilter))
        {
            dbQuery = dbQuery.Where(p =>
                p.Category != null &&
                p.Category.Name.ToLower().Contains(categoryFilter));
        }

        // Optional keyword filtering
        var keywords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (keywords.Any())
        {
            dbQuery = dbQuery.Where(p =>
                keywords.Any(k =>
                    p.Name.ToLower().Contains(k) ||
                    (p.Description != null && p.Description.ToLower().Contains(k))
                ));
        }

        var dbPlaces = await dbQuery
            .Take(4) // prioritize DB but limit
            .ToListAsync();

        // =========================
        // 🔹 3. GOOGLE PLACES SEARCH
        // =========================
        var googlePlaces = await _googlePlacesService.SearchAsync(query + " Lebanon");

        var mappedGoogle = googlePlaces.Select(g => new Place
        {
            Name = g.Name ?? "Unknown Place",
            Description = g.Address ?? "Popular place",
            Location = g.Address ?? "Lebanon",
            Category = new Category
            {
                Name = g.Types?.FirstOrDefault() ?? "Place"
            }
        }).ToList();

        // =========================
        // 🔹 4. MERGE + REMOVE DUPLICATES
        // =========================
        var allPlaces = dbPlaces
            .Concat(mappedGoogle)
            .GroupBy(p => p.Name.ToLower()) // avoid duplicates
            .Select(g => g.First())
            .Take(8)
            .ToList();

        return allPlaces;
    }

    // =========================
    // 🔹 FORMAT FOR AI (VERY IMPORTANT)
    // =========================
    public string FormatPlacesForPrompt(List<Place> places)
    {
        if (places == null || places.Count == 0)
            return "";

        var sb = new StringBuilder();

        sb.AppendLine("Relevant places you MUST prioritize in your answer:");

        foreach (var p in places)
        {
            sb.AppendLine($"- {p.Name} ({p.Category?.Name ?? "Place"}) in {p.Location}");

            if (!string.IsNullOrEmpty(p.Description))
                sb.AppendLine($"  Description: {p.Description}");
        }

        sb.AppendLine("IMPORTANT: Prefer these places instead of generic ones.");

        return sb.ToString();
    }
}