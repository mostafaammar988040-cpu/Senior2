using System.Text.RegularExpressions;

namespace Senior2.Api.Services;

public class GuardrailService
{
    // Lebanon-related keywords (expanded)
    private readonly HashSet<string> _lebanonKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "lebanon", "beirut", "byblos", "baalbek", "tyre", "sidon",
        "cedars", "jeita", "batroun", "jbeil", "anjar", "koura",
        "bekaa", "mount lebanon", "north lebanon", "south lebanon",
        "restaurant", "hotel", "guesthouse", "ski", "beach", "historical",
        "trip", "itinerary", "travel", "tour", "visit", "place",
        "food", "cuisine", "wine", "festival", "event", "culture",
        "nightlife", "shopping", "souk", "castle", "temple", "ruins",
        "mediterranean", "mountain", "valley", "waterfall", "cave",
        "lebanese", "mezze", "tabbouleh", "kibbeh", "shawarma",
        "covid", "safety", "transport", "car rental", "taxi"
    };

    // Known off-topic countries/regions (block if mentioned)
    private readonly HashSet<string> _otherCountries = new(StringComparer.OrdinalIgnoreCase)
    {
        "france", "paris", "germany", "berlin", "usa", "united states", "america", "new york",
        "uk", "london", "england", "britain", "spain", "madrid", "italy", "rome", "china",
        "beijing", "japan", "tokyo", "russia", "moscow", "india", "australia", "canada",
        "brazil", "mexico", "egypt", "cairo", "turkey", "istanbul", "greece", "athens"
    };

    // Always allow greetings, thanks, short follow-ups
    private readonly HashSet<string> _alwaysAllowed = new(StringComparer.OrdinalIgnoreCase)
    {
        "hello", "hi", "hey", "greetings", "good morning", "good afternoon",
        "good evening", "thanks", "thank you", "thx", "appreciate it",
        "bye", "goodbye", "see you", "ok", "okay", "sure", "yes", "no",
        "what's up", "how are you", "howdy", "help", "more", "another",
        "continue", "tell me more", "what else", "and"
    };

    public bool IsObviouslyOutOfScope(string message, bool hasHistory = false)
    {
        if (string.IsNullOrWhiteSpace(message))
            return true;

        var lower = message.ToLower().Trim();

        // If there's conversation history, assume context is set – allow everything except explicit off-topic.
        if (hasHistory)
        {
            // Only block if message contains a known other country and NO Lebanon keywords
            if (_otherCountries.Any(country => lower.Contains(country)) &&
                !_lebanonKeywords.Any(keyword => lower.Contains(keyword)))
                return true;
            else
                return false; // allow all else when history exists
        }

        // No history – first message or isolated query
        // Allow short greetings and common phrases
        if (_alwaysAllowed.Any(phrase => lower.Contains(phrase)))
            return false;

        // Check for other countries (with no Lebanon keywords)
        if (_otherCountries.Any(country => lower.Contains(country)) &&
            !_lebanonKeywords.Any(keyword => lower.Contains(keyword)))
            return true;

        // If message contains any Lebanon keyword, it's in scope
        if (_lebanonKeywords.Any(keyword => lower.Contains(keyword)))
            return false;

        // Very short messages (1-3 words) with no keywords – assume they might be follow-ups or vague,
        // but we'll allow them because they could be things like "tell me about it" – let the LLM handle.
        var words = lower.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= 3)
            return false; // allow

        // Longer messages with no Lebanon keywords – likely off-topic
        return true;
    }
}