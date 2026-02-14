using Senior2.Api.Models;

namespace Senior2.Api.Services;

public class ChatOrchestratorService
{
    private readonly IntentService _intentService;
    private readonly GuardrailService _guardrailService;
    private readonly WikipediaService _wikiService;
    private readonly OpenStreetMapService _osmService;
    private readonly LLMService _llmService;

    // Simple memory (per request scope)
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

        // 1️⃣ Guardrail
        if (_guardrailService.IsObviouslyOutOfScope(message))
        {
            return new ChatResponse
            {
                Reply = "I specialize in helping you explore Lebanon 🇱🇧. Please ask me something related to Lebanon.",
                Intent = "OutOfScope"
            };
        }

        // 2️⃣ Follow-up detection (ADD IT HERE)
        if (lower.Contains("another") ||
            lower.Contains("more") ||
            lower.Contains("continue"))
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

        // 3️⃣ Detect intent (THIS STAYS AFTER)
        var intent = _intentService.DetectIntent(message);


        // 4️⃣ History intent
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

            // Save topic for follow-ups
            _lastTopic = message;

            var formatted = await _llmService.FormatAsync(wikiData);

            return new ChatResponse
            {
                Reply = formatted,
                Intent = "History",
                Sources = new List<string> { "Wikipedia" }
            };
        }

        // 5️⃣ Location intent
        if (intent == "Location")
        {
            var places = await _osmService.SearchAsync(message);

            if (places == null || places.Count == 0)
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

        return new ChatResponse
        {
            Reply = "I'm not sure how to answer that yet.",
            Intent = "Unknown"
        };
    }
}
