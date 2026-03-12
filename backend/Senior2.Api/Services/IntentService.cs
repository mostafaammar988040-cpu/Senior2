namespace Senior2.Api.Services;

public class IntentService
{
    public string DetectIntent(string message)
    {
        var lower = message.ToLower();
        Console.WriteLine($"DetectIntent input: {message}");

        // Itinerary intent – check first
        if (lower.Contains("plan") ||
            lower.Contains("itinerary") ||
            lower.Contains("trip") ||
            lower.Contains("schedule") ||
            (lower.Contains("day") && (lower.Contains("in") || lower.Contains("to"))) ||
            (lower.Contains("suggest") && lower.Contains("itinerary")))
        {
            Console.WriteLine("Detected intent: Itinerary");
            return "Itinerary";
        }

        // Location intent
        if (lower.Contains("beach") || lower.Contains("restaurant") ||
            lower.Contains("hotel") || lower.Contains("hospital") ||
            lower.Contains("where"))
        {
            Console.WriteLine("Detected intent: Location");
            return "Location";
        }

        // History intent
        if (lower.Contains("history") || lower.Contains("tell me about") ||
            lower.Contains("what is") || lower.Contains("who is") ||
            lower.Contains("when") || lower.Contains("about"))
        {
            Console.WriteLine("Detected intent: History");
            return "History";
        }

        Console.WriteLine("Detected intent: History (default)");
        return "History";
    }
}