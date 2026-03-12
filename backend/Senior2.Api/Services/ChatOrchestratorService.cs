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
        // Store user message if session exists
        if (!string.IsNullOrEmpty(sessionId))
            _memoryService.AddMessage(sessionId, "user", message);

        var lower = message.ToLower();

        // 1) Guardrail
        if (_guardrailService.IsObviouslyOutOfScope(message))
        {
            var reply = "I specialize in helping you explore Lebanon 🇱🇧. Please ask me something related to Lebanon.";
            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", reply);
            return new ChatResponse { Reply = reply, Intent = "OutOfScope" };
        }

        // 🔍 Search database for relevant places
        var relevantPlaces = await _placeSearchService.SearchPlacesAsync(message);
        string placeContext = _placeSearchService.FormatPlacesForPrompt(relevantPlaces);

        // 2) Follow-up detection
        if (lower.Contains("another") || lower.Contains("more") || lower.Contains("continue"))
        {
            if (_lastTopic != null)
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

            var fallback = "Please specify what topic you'd like more information about.";
            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", fallback);
            return new ChatResponse { Reply = fallback, Intent = "Unknown" };
        }

        // 3) Detect intent
        var intent = _intentService.DetectIntent(message);

        if (intent == "Itinerary")
{
    var itinerary = await _itineraryService.GenerateItineraryAsync(message);
    if (!string.IsNullOrEmpty(sessionId))
        _memoryService.AddMessage(sessionId, "assistant", itinerary);
    return new ChatResponse { Reply = itinerary, Intent = "Itinerary" };
}

        // 4) History intent
        if (intent == "History")
        {
            var wikiData = await _wikiService.GetInfoAsync(message);
            if (wikiData == null)
            {
                var reply = await _llmService.GetChatResponseAsync(message, placeContext);
                if (!string.IsNullOrEmpty(sessionId))
                    _memoryService.AddMessage(sessionId, "assistant", reply);
                return new ChatResponse { Reply = reply, Intent = "History" };
            }

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

        // 5) Location intent
        if (intent == "Location")
        {
            string searchQuery = message.ToLower();
            if (searchQuery.Contains("restaurant")) searchQuery = "restaurant beirut";
            if (searchQuery.Contains("coffee") || searchQuery.Contains("cafe")) searchQuery = "cafe beirut";
            if (searchQuery.Contains("hotel")) searchQuery = "hotel beirut";

            var osmResults = await _osmService.SearchPlaces(searchQuery, 5);
            var places = osmResults.Select(p => new PlaceResult
            {
                Name = p.GetType().GetProperty("name")?.GetValue(p)?.ToString() ?? "",
                Type = "Place",
                City = p.GetType().GetProperty("location")?.GetValue(p)?.ToString() ?? ""
            }).ToList();

            if (places.Count == 0)
            {
                var reply = await _llmService.GetChatResponseAsync(message, placeContext);
                if (!string.IsNullOrEmpty(sessionId))
                    _memoryService.AddMessage(sessionId, "assistant", reply);
                return new ChatResponse { Reply = reply, Intent = "Location" };
            }

            _lastTopic = message;
            var replyMessage = "Here are some great places in Lebanon:";
            if (!string.IsNullOrEmpty(sessionId))
                _memoryService.AddMessage(sessionId, "assistant", replyMessage);
            return new ChatResponse
            {
                Reply = replyMessage,
                Intent = "Location",
                Places = places
            };
        }

        // 6) Default fallback – use LLM with context AND history
        var history = string.IsNullOrEmpty(sessionId) ? null : _memoryService.GetHistory(sessionId);
        var finalReply = await _llmService.GetChatResponseAsync(message, placeContext, history);
        if (!string.IsNullOrEmpty(sessionId))
            _memoryService.AddMessage(sessionId, "assistant", finalReply);
        return new ChatResponse { Reply = finalReply, Intent = "Unknown" };
    }
}