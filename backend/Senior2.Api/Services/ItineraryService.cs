using System.Text.RegularExpressions;

namespace Senior2.Api.Services
{
    public class ItineraryService
    {
        private readonly LLMService _llmService;
        private readonly RecommendationService _recommendationService;

        private readonly HashSet<string> _lebanonDestinations = new(StringComparer.OrdinalIgnoreCase)
        {
            "lebanon",
            "beirut", "byblos", "jbeil", "baalbek", "tyre", "sour", "sidon", "saida",
            "batroun", "jeita", "cedars", "arz", "jounieh", "anjar",
            "zahle", "bekaa", "tripoli", "bcharre", "ehden", "koura",
            "amchit", "harissa", "faraya", "faqra", "deir el qamar",
            "beiteddine", "chouf", "niha", "qadisha", "tannourine"
        };

        public ItineraryService(
            LLMService llmService,
            RecommendationService recommendationService)
        {
            _llmService = llmService;
            _recommendationService = recommendationService;
        }

        public async Task<string> GenerateItineraryAsync(
            string userMessage,
            string tripType,
            int budget,
            string travelerType)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return "Please provide a trip request so I can create an itinerary for Lebanon.";
            }

            Console.WriteLine($"GenerateItineraryAsync called with message: {userMessage}");

            var (destination, days) = await _llmService.ExtractItineraryParams(userMessage);

            if (string.IsNullOrWhiteSpace(destination))
            {
                destination = ExtractDestinationFromMessage(userMessage);
            }

            destination = string.IsNullOrWhiteSpace(destination)
                ? "Lebanon"
                : NormalizeDestination(destination);

            int numberOfDays = days ?? ExtractDaysFromMessage(userMessage) ?? 2;

            if (numberOfDays < 1)
                numberOfDays = 1;

            if (numberOfDays > 7)
                numberOfDays = 7;

            tripType = string.IsNullOrWhiteSpace(tripType) ? "leisure" : tripType;
            travelerType = string.IsNullOrWhiteSpace(travelerType) ? "any" : travelerType;

            if (budget <= 0)
                budget = 200;

            Console.WriteLine(
                $"Final itinerary values: destination={destination}, days={numberOfDays}, tripType={tripType}, budget={budget}, travelerType={travelerType}"
            );

            var recommendations = await _recommendationService.GetItineraryRecommendation(
                destination,
                tripType,
                budget,
                travelerType
            );

            var destinationRules = GetDestinationRules(destination);

            var prompt = $@"
You are an AI travel planner for a Lebanon tourism platform.

Create a {numberOfDays}-day travel itinerary.

User request:
{userMessage}

Trip details:
- Destination: {destination}, Lebanon
- Number of days: {numberOfDays}
- Trip type: {tripType}
- Budget per day: ${budget}
- Traveler type: {travelerType}

Useful recommendations:
{recommendations}

Important destination rules:
{destinationRules}

Return the answer in this exact readable format:

Day 1:
- Morning: [real place/activity + short reason]
- Afternoon: [real place/activity + short reason]
- Evening: [real place/activity + short reason]
- Restaurant: [real restaurant or realistic food area in the same destination]
- Estimated Budget: [short estimate]
- Note: [short practical note]

Rules:
- Create exactly {numberOfDays} days.
- Keep the itinerary inside or very near {destination}.
- Do not suggest far cities unless the user requested a multi-city trip.
- If you mention a place outside {destination}, label it clearly as an optional nearby day trip.
- Do not repeat the same activity across multiple days.
- Include one restaurant or food suggestion every day.
- Prefer real places and known areas.
- Do not invent fake restaurant names.
- If you are unsure about a restaurant name, write: local restaurant in {destination}.
- Match the trip type, budget, and traveler type.
- If the trip is cheap or budget, avoid luxury resorts and expensive restaurants.
- If the trip is luxury, include premium restaurants, rooftops, resorts, or private experiences.
- If the trip is adventure, include nature, hiking, outdoor activities, or beaches.
- If the trip is romantic, include sunset spots, calm restaurants, sea views, or old towns.
- If the trip is cultural, include museums, ruins, castles, souks, or heritage sites.
- If the traveler type is family, avoid nightlife-focused suggestions.
- Keep each line short and clear.
- Do not write long paragraphs.
";

            var itinerary = await _llmService.GetChatResponseAsync(prompt);

            return CleanResponse(itinerary);
        }

        private string? ExtractDestinationFromMessage(string message)
        {
            var lower = message.ToLower();

            foreach (var destination in _lebanonDestinations)
            {
                if (lower.Contains(destination.ToLower()))
                {
                    Console.WriteLine($"Fallback found destination: {destination}");
                    return destination;
                }
            }

            return null;
        }

        private int? ExtractDaysFromMessage(string message)
        {
            var match = Regex.Match(
                message.ToLower(),
                @"\b(\d+)\s*(day|days)\b"
            );

            if (match.Success && int.TryParse(match.Groups[1].Value, out int days))
            {
                return days;
            }

            return null;
        }

        private string NormalizeDestination(string destination)
        {
            destination = destination.Trim();

            if (destination.Equals("jbeil", StringComparison.OrdinalIgnoreCase))
                return "Byblos";

            if (destination.Equals("saida", StringComparison.OrdinalIgnoreCase))
                return "Sidon";

            if (destination.Equals("sour", StringComparison.OrdinalIgnoreCase))
                return "Tyre";

            if (destination.Equals("arz", StringComparison.OrdinalIgnoreCase))
                return "Cedars";

            if (destination.Equals("qadisha", StringComparison.OrdinalIgnoreCase))
                return "Qadisha Valley";

            if (destination.Equals("deir el qamar", StringComparison.OrdinalIgnoreCase))
                return "Deir El Qamar";

            return char.ToUpper(destination[0]) + destination.Substring(1).ToLower();
        }

        private string GetDestinationRules(string destination)
        {
            var lower = destination.ToLower();

            if (lower.Contains("ehden"))
            {
                return @"
- For Ehden, prioritize Horsh Ehden Nature Reserve, Ehden old town, Saydet El Hosn, nearby Zgharta, Qozhaya Monastery, Qadisha Valley, Bcharre, and Cedars.
- Do not suggest Batroun, Tyre, Beirut, or far coastal beaches for an Ehden-only trip.
- For cheap adventure in Ehden, focus on hiking, nature, viewpoints, picnics, and local Lebanese food.
";
            }

            if (lower.Contains("tyre"))
            {
                return @"
- For Tyre, prioritize Tyre Public Beach, Tyre Coast Nature Reserve, Tyre old town, Al Mina area, Tyre Roman ruins, Tyre Hippodrome, and seafood by the coast.
- Do not suggest Lazy B, Batroun, Beirut, or Byblos for a Tyre-only trip.
- For beaches and food in Tyre, focus on public beaches, seafood, old souk, and coastal walks.
";
            }

            if (lower.Contains("batroun"))
            {
                return @"
- For Batroun, prioritize Batroun Old Souk, Phoenician Wall, Batroun beach, Colonel Beer area, seaside cafés, St. Stephen Cathedral, and nearby Kfarabida if needed.
- Do not suggest Tyre, Ehden, or Beirut for a Batroun-only trip.
- Byblos can be mentioned only as an optional nearby day trip.
";
            }

            if (lower.Contains("byblos"))
            {
                return @"
- For Byblos, prioritize Byblos Castle, Byblos Old Souk, Byblos Port, archaeological site, old town, seaside restaurants, and nearby beaches.
- Do not suggest Tyre, Ehden, or Beirut for a Byblos-only trip.
- Batroun can be mentioned only as an optional nearby day trip.
";
            }

            if (lower.Contains("beirut"))
            {
                return @"
- For Beirut, prioritize Raouche, Beirut Souks, Zaitunay Bay, National Museum, Sursock Museum, Gemmayze, Mar Mikhael, Hamra, Downtown, and the Corniche.
- Do not suggest far mountain or coastal cities unless the user requests day trips.
";
            }

            if (lower.Contains("baalbek"))
            {
                return @"
- For Baalbek, prioritize Baalbek Roman Temples, Stone of the Pregnant Woman, local souks, Bekaa food experiences, Zahle, and nearby wineries if suitable.
- Do not suggest Beirut, Batroun, or Tyre for a Baalbek-only cultural trip.
";
            }

            if (lower.Contains("lebanon"))
            {
                return @"
- For a general Lebanon itinerary, distribute the days across different regions logically.
- Use nearby regions together to avoid unrealistic travel.
- Example grouping: Beirut and Mount Lebanon together, Byblos and Batroun together, Ehden and Cedars together, Tyre and Sidon together, Baalbek and Zahle together.
";
            }

            return @"
- Prioritize attractions inside the requested destination.
- Avoid far cities unless the user requested multiple regions.
- Keep travel realistic for Lebanon distances.
";
        }

        private string CleanResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return "Sorry, I could not generate an itinerary. Please try again.";

            var cleaned = response.Trim();

            cleaned = cleaned.Replace("```json", "", StringComparison.OrdinalIgnoreCase);
            cleaned = cleaned.Replace("```", "");

            return cleaned.Trim();
        }
    }
}