using Senior2.Api.Models;

namespace Senior2.Api.Services;

public class ChatOrchestratorService
{
    private readonly IntentService _intentService;
    private readonly GuardrailService _guardrailService;
    private readonly WikipediaService _wikiService;
    private readonly OpenStreetMapService _osmService;
    private readonly LLMService _llmService;
    private readonly PlaceSearchService _placeSearchService;
    private readonly ConversationMemoryService _memoryService;
    private readonly ItineraryService _itineraryService;

    private string? _lastTopic;

    public ChatOrchestratorService(
        IntentService intentService,
        GuardrailService guardrailService,
        WikipediaService wikiService,
        OpenStreetMapService osmService,
        LLMService llmService,
        PlaceSearchService placeSearchService,
        ConversationMemoryService memoryService,
        ItineraryService itineraryService)
    {
        _intentService = intentService;
        _guardrailService = guardrailService;
        _wikiService = wikiService;
        _osmService = osmService;
        _llmService = llmService;
        _placeSearchService = placeSearchService;
        _memoryService = memoryService;
        _itineraryService = itineraryService;
    }

    public async Task<ChatResponse> ProcessAsync(string message, string? sessionId = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return new ChatResponse
            {
                Reply = "Please type a message so I can help you explore Lebanon 🇱🇧.",
                Intent = "Unknown"
            };
        }

        var lower = message.ToLower().Trim();

        if (!string.IsNullOrEmpty(sessionId))
            _memoryService.AddMessage(sessionId, "user", message);

        var hasHistory = !string.IsNullOrEmpty(sessionId) &&
                         _memoryService.GetHistory(sessionId).Any();

        if (_guardrailService.IsObviouslyOutOfScope(message, hasHistory))
        {
            var reply = "I specialize in helping you explore Lebanon 🇱🇧. Please ask me something related to Lebanon.";

            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", reply);

            return new ChatResponse
            {
                Reply = reply,
                Intent = "OutOfScope"
            };
        }

        var relevantPlaces = await _placeSearchService.SearchPlacesAsync(message);
        var placeContext = _placeSearchService.FormatPlacesForPrompt(relevantPlaces);

        if (lower.Contains("another") || lower.Contains("more") || lower.Contains("continue"))
        {
            if (!string.IsNullOrEmpty(_lastTopic))
            {
                var wikiData = await _wikiService.GetInfoAsync(_lastTopic);

                if (wikiData != null)
                {
                    var formatted = await _llmService.FormatAsync(wikiData, placeContext);

                    if (!string.IsNullOrEmpty(sessionId))
                        _memoryService.AddMessage(sessionId, "assistant", formatted);

                    return new ChatResponse
                    {
                        Reply = formatted,
                        Intent = "History",
                        Sources = new List<string> { "Wikipedia" }
                    };
                }
            }

            var fallback = "Sure — can you tell me what you want more about? A place, restaurant, cafe, activity, or itinerary in Lebanon?";

            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", fallback);

            return new ChatResponse
            {
                Reply = fallback,
                Intent = "Unknown"
            };
        }

        var intent = _intentService.DetectIntent(message);

        var enhancedMessage = message;

        if (intent == "Location")
        {
            enhancedMessage = $"Suggest places in Lebanon for this request: {message}. If the exact location is unclear, suggest nearby options in Lebanon.";
        }
      

        if (intent == "Itinerary")
        {
            string tripType = "leisure";
            int budget = 200;
            string travelerType = "any";

            if (lower.Contains("business")) tripType = "business";
            else if (lower.Contains("hiking") || lower.Contains("adventure") || lower.Contains("trek")) tripType = "adventure";
            else if (lower.Contains("romantic") || lower.Contains("honeymoon")) tripType = "romantic";
            else if (lower.Contains("cultural") || lower.Contains("history") || lower.Contains("museum")) tripType = "cultural";

            if (lower.Contains("luxury") || lower.Contains("expensive")) budget = 1000;
            else if (lower.Contains("cheap") || lower.Contains("budget") || lower.Contains("low cost")) budget = 100;
            else if (lower.Contains("moderate") || lower.Contains("mid") || lower.Contains("mid-range")) budget = 300;

            if (lower.Contains("family")) travelerType = "family";
            else if (lower.Contains("solo")) travelerType = "solo";
            else if (lower.Contains("couple")) travelerType = "couple";
            else if (lower.Contains("friends")) travelerType = "group";

            var itinerary = await _itineraryService.GenerateItineraryAsync(
     message,
     tripType,
     budget,
     travelerType
 );
            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", itinerary);

            return new ChatResponse
            {
                Reply = itinerary,
                Intent = "Itinerary"
            };
        }

        if (intent == "History")
        {
            var wikiData = await _wikiService.GetInfoAsync(message);

            if (wikiData != null)
            {
                _lastTopic = message;

                var formatted = await _llmService.FormatAsync(wikiData, placeContext);

                if (!string.IsNullOrEmpty(sessionId))
                    _memoryService.AddMessage(sessionId, "assistant", formatted);

                return new ChatResponse
                {
                    Reply = formatted,
                    Intent = "History",
                    Sources = new List<string> { "Wikipedia" }
                };
            }

            var history = string.IsNullOrEmpty(sessionId)
                ? null
                : _memoryService.GetHistory(sessionId);

            var reply = await _llmService.GetChatResponseAsync(
                enhancedMessage,
                placeContext,
                history
            );

            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", reply);

            return new ChatResponse
            {
                Reply = reply,
                Intent = "History"
            };
        }

        if (intent == "Location")
        {
            string searchQuery;

            if (lower.Contains("restaurant"))
                searchQuery = $"{message} restaurant Lebanon";
            else if (lower.Contains("coffee") || lower.Contains("cafe"))
                searchQuery = $"{message} cafe Lebanon";
            else if (lower.Contains("hotel"))
                searchQuery = $"{message} hotel Lebanon";
            else
                searchQuery = $"{message} Lebanon";

            var osmResults = await _osmService.SearchPlaces(searchQuery, 5);

            var places = osmResults.Select(p => new PlaceResult
            {
                Name = p.GetType().GetProperty("name")?.GetValue(p)?.ToString() ?? "Unnamed place",
                Type = "Place",
                City = p.GetType().GetProperty("location")?.GetValue(p)?.ToString() ?? "Lebanon"
            }).ToList();

            if (places.Count > 0)
            {
                _lastTopic = message;

                var replyMessage = $"Here are some places I found for \"{message}\" 🇱🇧:";

                if (!string.IsNullOrEmpty(sessionId))
                    _memoryService.AddMessage(sessionId, "assistant", replyMessage);

                return new ChatResponse
                {
                    Reply = replyMessage,
                    Intent = "Location",
                    Places = places
                };
            }

            var history = string.IsNullOrEmpty(sessionId)
                ? null
                : _memoryService.GetHistory(sessionId);

            var reply = await _llmService.GetChatResponseAsync(
                $"Suggest helpful alternatives in Lebanon for this request: {message}",
                placeContext,
                history
            );

            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", reply);

            return new ChatResponse
            {
                Reply = reply,
                Intent = "Location"
            };
        }

        var conversationHistory = string.IsNullOrEmpty(sessionId)
            ? null
            : _memoryService.GetHistory(sessionId);

        var finalReply = await _llmService.GetChatResponseAsync(
            enhancedMessage,
            placeContext,
            conversationHistory
        );

        if (!string.IsNullOrEmpty(sessionId))
            _memoryService.AddMessage(sessionId, "assistant", finalReply);

        return new ChatResponse
        {
            Reply = finalReply,
            Intent = intent
        };
    }
}