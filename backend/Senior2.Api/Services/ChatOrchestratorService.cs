using Senior2.Api.Models;

namespace Senior2.Api.Services;

public class ChatOrchestratorService
{
    private readonly IntentService _intentService;
    private readonly GuardrailService _guardrailService;
    private readonly WikipediaService _wikiService;
    private readonly OpenStreetMapService _osmService;
    private readonly LLMService _llmService;

    private string? _lastTopic;

    public ChatOrchestratorService(
        IntentService intentService,
        GuardrailService guardrailService,
        WikipediaService wikiService,
        OpenStreetMapService osmService,
        LLMService llmService)
    {
        _intentService = intentService;
        _guardrailService = guardrailService;
        _wikiService = wikiService;
        _osmService = osmService;
        _llmService = llmService;
    }

    public async Task<ChatResponse> ProcessAsync(string message)
    {
        var lower = message.ToLower();

        // 1) Guardrail
        if (_guardrailService.IsObviouslyOutOfScope(message))
        {
            return new ChatResponse
            {
                Reply = "I specialize in helping you explore Lebanon 🇱🇧. Please ask me something related to Lebanon.",
                Intent = "OutOfScope"
            };
        }

        // 2) Follow-up detection
        if (lower.Contains("another") || lower.Contains("more") || lower.Contains("continue"))
        {
            if (_lastTopic != null)
            {
                var wikiData = await _wikiService.GetInfoAsync(_lastTopic);

                if (wikiData != null)
                {
                    var formatted = await _llmService.FormatAsync(wikiData);

                    return new ChatResponse
                    {
                        Reply = formatted,
                        Intent = "History",
                        Sources = new List<string> { "Wikipedia" }
                    };
                }
            }

            return new ChatResponse
            {
                Reply = "Please specify what topic you'd like more information about.",
                Intent = "Unknown"
            };
        }

        // 3) Detect intent
        var intent = _intentService.DetectIntent(message);

        // 4) History intent
        if (intent == "History")
        {
            var wikiData = await _wikiService.GetInfoAsync(message);

            if (wikiData == null)
            {
                return new ChatResponse
                {
                    Reply = "I specialize in helping you explore Lebanon 🇱🇧. Please ask about Lebanese places or history.",
                    Intent = "OutOfScope"
                };
            }

            _lastTopic = message;

            var formatted = await _llmService.FormatAsync(wikiData);

            return new ChatResponse
            {
                Reply = formatted,
                Intent = "History",
                Sources = new List<string> { "Wikipedia" }
            };
        }

        // 5) Location intent
        if (intent == "Location")
        {
            string searchQuery = message.ToLower();

            if (searchQuery.Contains("restaurant"))
                searchQuery = "restaurant beirut";

            if (searchQuery.Contains("coffee") || searchQuery.Contains("cafe"))
                searchQuery = "cafe beirut";

            if (searchQuery.Contains("hotel"))
                searchQuery = "hotel beirut";

            var osmResults = await _osmService.SearchPlaces(searchQuery, 5);
            var places = osmResults.Select(p => new PlaceResult
            {
                Name = p.GetType().GetProperty("name")?.GetValue(p)?.ToString() ?? "",
                Type = "Place",
                City = p.GetType().GetProperty("location")?.GetValue(p)?.ToString() ?? ""
            }).ToList();

            if (places.Count == 0)
            {
                return new ChatResponse
                {
                    Reply = "I couldn't find matching locations in Lebanon. Try being more specific.",
                    Intent = "Location"
                };
            }

            _lastTopic = message;

            return new ChatResponse
            {
                Reply = "Here are some great places in Lebanon:",
                Intent = "Location",
                Places = places
            };
        }

        // 6) Default fallback
        return new ChatResponse
        {
            Reply = "I'm not sure how to answer that yet.",
            Intent = "Unknown"
        };
    }
}