namespace Senior2.Api.Services;

public class LLMService
{
    public async Task<string> FormatAsync(string text)
    {
        // Temporary formatting (later we connect OpenAI)
        return await Task.FromResult($"Here is what I found:\n\n{text}");
    }
}
