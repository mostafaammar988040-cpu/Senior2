using System.Linq;
namespace Senior2.Api.Services;

public class GuardrailService
{
    public bool IsObviouslyOutOfScope(string message)
{
    var lower = message.ToLower();

    // Lebanon tourism keywords (expand as needed)
    var lebanonKeywords = new[]
    {
        "lebanon", "beirut", "byblos", "baalbek", "tyre", "sidon",
        "cedars", "jeita", "batroun", "jbeil", "anjar", "koura",
        "bekaa", "mount lebanon", "north lebanon", "south lebanon",
        "restaurant", "hotel", "guesthouse", "ski", "beach", "historical",
        "trip", "itinerary", "travel", "tour", "visit", "place",
        "food", "cuisine", "wine", "festival", "event", "culture"
    };

    // If any keyword is present, it's in scope
    if (lebanonKeywords.Any(keyword => lower.Contains(keyword)))
        return false;   // within scope

    // Otherwise, reject
    return true;
}
}
