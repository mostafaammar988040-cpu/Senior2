namespace Senior2.Api.Services;

public class IntentService
{
    public string DetectIntent(string message)
    {
        var lower = message.ToLower();

        // Location intent
        if (lower.Contains("beach") ||
            lower.Contains("restaurant") ||
            lower.Contains("hotel") ||
            lower.Contains("hospital") ||
            lower.Contains("where"))
            return "Location";

        // Informational / History intent
        if (lower.Contains("history") ||
            lower.Contains("tell me about") ||
            lower.Contains("what is") ||
            lower.Contains("who is") ||
            lower.Contains("when") ||
            lower.Contains("about"))
            return "History";

        // Default: treat unknown as informational
        return "History";
    }
}
