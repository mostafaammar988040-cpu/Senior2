using Senior2.Api.Services;
using System.Text.Json;

namespace Senior2.Api.Services
{
    public class ItineraryService
    {
        private readonly LLMService _llmService;
        private readonly RecommendationService _recommendationService;

        private readonly HashSet<string> _lebanonDestinations = new(StringComparer.OrdinalIgnoreCase)
        {
            "beirut", "byblos", "jbeil", "baalbek", "tyre", "sidon", "saida",
            "batroun", "jeita", "cedars", "arz", "jounieh", "anjar",
            "zahle", "bekaa", "tripoli", "bcharre", "ehden", "koura", "amchit"
        };

        public ItineraryService(LLMService llmService, RecommendationService recommendationService)
        {
            _llmService = llmService;
            _recommendationService = recommendationService;
        }

        public async Task<string> GenerateItineraryAsync(string userMessage, string tripType, int budget, string travelerType)
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

            // 🔥 Use OpenAI via RecommendationService for itinerary suggestions
            var aiSuggestions = await _recommendationService.GetItineraryRecommendation(
                destination,
                tripType,
                budget,
                travelerType
            );

            // Build a structured itinerary object
            var itinerary = new List<object>();
            for (int day = 1; day <= numberOfDays; day++)
            {
                itinerary.Add(new
                {
                    day,
                    region = destination,
                    aiRecommendations = aiSuggestions // reuse suggestions for now
                });
            }

            // Serialize to JSON for saving/display
            return JsonSerializer.Serialize(itinerary);
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
}