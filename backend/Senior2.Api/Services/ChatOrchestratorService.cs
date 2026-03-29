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
        /*
        PSEUDOCODE / PLAN (detailed):
        1. Store the user message in conversation memory if sessionId provided.
        2. Run a guardrail check; if out of scope return a canned reply and record it.
        3. Search for relevant places and format them for prompts.
        4. Handle simple follow-up keywords ("another", "more", "continue"):
           - If there is a last topic, fetch Wikipedia info and format it with LLM.
           - Otherwise return a fallback asking the user to specify a topic.
        5. Detect intent using IntentService.
           - If intent == "Itinerary":
             a. Build values for the required parameters (`tripType`, `budget`, `travelerType`) with safe defaults.
             b. Apply small, conservative heuristics to adjust those defaults based on keywords in the message.
             c. Call `GenerateItineraryAsync(message, tripType, budget, travelerType)`.
             d. Store and return the itinerary response.
           - If intent == "History":
             a. Query WikipediaService for the message/topic.
             b. If found, set _lastTopic and format with LLM, otherwise ask LLM directly.
           - If intent == "Location":
             a. Map simple keywords to focused OSM search queries (restaurant, cafe, hotel).
             b. Query OSM and map results to PlaceResult objects.
             c. If no places found, fall back to LLM chat response.
           - Default:
             a. Get history if session exists and call LLM with message, placeContext, and history.
             b. Store and return finalReply.
        NOTES:
        - The original compile error was due to calling GenerateItineraryAsync with only one argument while the method requires four.
        - Fix: provide the missing arguments. Here we use conservative defaults and light heuristics derived from the user's message.
        - This approach avoids modifying ItineraryService signatures and keeps the fix localized.
        */

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
            // Provide required parameters for GenerateItineraryAsync(string userMessage, string tripType, int budget, string travelerType)
            // Use conservative defaults and light heuristics based on keywords in the user's message.
            string tripType = "leisure";
            int budget = 200; // default approximate budget
            string travelerType = "any";

            var lowerMsg = message.ToLower();

            // Heuristics for tripType
            if (lowerMsg.Contains("business")) tripType = "business";
            else if (lowerMsg.Contains("hiking") || lowerMsg.Contains("adventure") || lowerMsg.Contains("trek")) tripType = "adventure";
            else if (lowerMsg.Contains("romantic") || lowerMsg.Contains("honeymoon")) tripType = "romantic";
            else if (lowerMsg.Contains("cultural") || lowerMsg.Contains("history") || lowerMsg.Contains("museum")) tripType = "cultural";

            // Heuristics for budget
            if (lowerMsg.Contains("luxury") || lowerMsg.Contains("expensive")) budget = 1000;
            else if (lowerMsg.Contains("cheap") || lowerMsg.Contains("budget") || lowerMsg.Contains("low cost")) budget = 100;
            else if (lowerMsg.Contains("moderate") || lowerMsg.Contains("mid") || lowerMsg.Contains("mid-range")) budget = 300;

            // Heuristics for travelerType
            if (lowerMsg.Contains("family")) travelerType = "family";
            else if (lowerMsg.Contains("solo")) travelerType = "solo";
            else if (lowerMsg.Contains("couple")) travelerType = "couple";
            else if (lowerMsg.Contains("friends")) travelerType = "group";

            var itinerary = await _itineraryService.GenerateItineraryAsync(message, tripType, budget, travelerType);
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