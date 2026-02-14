namespace Senior2.Api.Services;

public class GuardrailService
{
    public bool IsObviouslyOutOfScope(string message)
    {
        var lower = message.ToLower();

        // Reject clearly non-Lebanon topics
        if (lower.Contains("germany") ||
            lower.Contains("france") ||
            lower.Contains("usa") ||
            lower.Contains("china") ||
            lower.Contains("japan"))
        {
        }
            return true;

        return false;
    }
}
