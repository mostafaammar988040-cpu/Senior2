using System.Text;
using Senior2.Api.Services;

namespace Senior2.Api.Services;

public class ItineraryService
{
    private readonly LLMService _llmService;

    private readonly HashSet<string> _lebanonDestinations = new(StringComparer.OrdinalIgnoreCase)
    {
        "beirut", "byblos", "jbeil", "baalbek", "tyre", "sidon", "saida",
        "batroun", "jeita", "cedars", "arz", "jounieh", "jbeil", "anjar",
        "zahle", "bekaa", "tripoli", "bcharre", "ehden", "koura", "amchit"
    };

    public ItineraryService(LLMService llmService)
    {
        _llmService = llmService;
    }

    public async Task<string> GenerateItineraryAsync(string userMessage)
{
    Console.WriteLine($"GenerateItineraryAsync called with message: {userMessage}");

    var (destination, days) = await _llmService.ExtractItineraryParams(userMessage);
    Console.WriteLine($"Extracted: destination='{destination}', days={days?.ToString() ?? "null"}");

    if (string.IsNullOrEmpty(destination))
    {
        destination = ExtractDestinationFromMessage(userMessage);
        Console.WriteLine($"Fallback destination: '{destination}'");
    }

    destination = string.IsNullOrEmpty(destination) ? "Beirut" : destination;
    int numberOfDays = days ?? 2;

    Console.WriteLine($"Final: destination='{destination}', days={numberOfDays}");

    // Use the dedicated LLM method for itinerary generation
    var itinerary = await _llmService.GenerateItineraryAsync(destination, numberOfDays);
    return itinerary;
}

   private async Task<string> GenerateItineraryWithLLM(string destination, int days)
{
    var prompt = $@"Create a detailed {days}-day travel itinerary for {destination}, Lebanon. 
The itinerary should include a mix of historical sites, cultural experiences, local cuisine, and nature spots.
For each day, provide 2-3 specific activities or places to visit, along with brief descriptions.
Make it varied and interesting – avoid repeating the same type of activity every day.
Include recommendations for breakfast, lunch, and dinner where appropriate.
Format the response with clear day headings and bullet points and make it look organised and easy to read";

    return await _llmService.GenerateItineraryAsync(prompt);
}

    private string ExtractDestinationFromMessage(string message)
    {
        var lower = message.ToLower();
        foreach (var dest in _lebanonDestinations)
        {
            if (lower.Contains(dest))
            {
                Console.WriteLine($"Fallback found destination: {dest}");
                return dest;
            }
        }
        return null;
    }
}