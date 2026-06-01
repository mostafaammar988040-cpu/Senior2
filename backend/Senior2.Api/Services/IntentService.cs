using System.Text.RegularExpressions;

namespace Senior2.Api.Services;

public class IntentService
{
    public string DetectIntent(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return "Unknown";

        var lower = message.ToLower();

        if (Regex.IsMatch(lower, @"\b(plan|itinerary|trip|schedule)\b") ||
            Regex.IsMatch(lower, @"\b\d+\s*(day|days)\b"))
        {
            return "Itinerary";
        }

        if (
     Regex.IsMatch(lower, @"\b(restaurant|cafe|coffee|bar|hotel|place|spot|visit|where)\b") ||
     Regex.IsMatch(lower, @"\b(dinner|lunch|breakfast|drink|hangout|chill|go out|activity|things to do)\b")
 )
        {
            return "Location";
        }

        if (Regex.IsMatch(lower, @"\b(tell me about|what is|who is|history|when)\b"))
        {
            return "History";
        }

        return "General";
    }
}