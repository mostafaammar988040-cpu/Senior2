namespace Senior2.Api.Services;

public class IntentService
{
    public string DetectIntent(string message)
    {
        var lower = message.ToLower();

        if (lower.Contains("beach") ||
            lower.Contains("restaurant") ||
            lower.Contains("food") ||
            lower.Contains("eat") ||
            lower.Contains("cafe") ||
            lower.Contains("coffee") ||
            lower.Contains("coffee shop") ||
            lower.Contains("hotel") ||
            lower.Contains("guesthouse") ||
            lower.Contains("hospital") ||
            lower.Contains("where") ||
            lower.Contains("recommend"))
            return "Location";

        if (lower.Contains("history") ||
            lower.Contains("tell me about") ||
            lower.Contains("what is") ||
            lower.Contains("who is") ||
            lower.Contains("when") ||
            lower.Contains("about"))
            return "History";

        return "History";
    }
}