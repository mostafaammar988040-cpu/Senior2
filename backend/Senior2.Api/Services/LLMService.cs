using OpenAI.Chat;
using System.Text.Json;
namespace Senior2.Api.Services;

public class LLMService
{
    private readonly ChatClient _chatClient;

    public LLMService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"] 
            ?? throw new InvalidOperationException("OpenAI API key not found.");
        _chatClient = new ChatClient(model: "gpt-4o-mini", apiKey);
    }

    // Main method with optional context and conversation history
    public async Task<string> GetChatResponseAsync(string userMessage, string context = "", List<ChatMessage>? history = null)
    {
        var messages = new List<ChatMessage>();

        // System prompt (Lebanon-only focus)
        messages.Add(new SystemChatMessage(@"
You are a knowledgeable and friendly travel assistant specialized **only** in Lebanon tourism, history, culture, food, and travel planning. 
If the user asks about anything not related to Lebanon (e.g., other countries, general knowledge, mathematics, etc.), politely decline and redirect them to ask about Lebanon.
Keep answers concise, helpful, and focused on Lebanon. They can ask about places that represent or serve something non-lebanese, but the place should be in Lebanon."));

        // Add database context if provided
        if (!string.IsNullOrEmpty(context))
        {
            messages.Add(new SystemChatMessage($"Relevant information from our database:\n{context}"));
        }

        // Add conversation history if available (oldest first)
        if (history != null && history.Any())
        {
            messages.AddRange(history);
        }

        // Add current user message
        messages.Add(new UserChatMessage(userMessage));

        var completion = await _chatClient.CompleteChatAsync(messages);
        return completion.Value.Content[0].Text;
    }

    // For Wikipedia formatting with optional context
    public async Task<string> FormatWikiAsync(string wikiText, string userQuery, string context = "")
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a Lebanon travel expert. Summarize the following Wikipedia information in a helpful way for a traveler, focusing on practical details relevant to visiting Lebanon.")
        };

        if (!string.IsNullOrEmpty(context))
        {
            messages.Add(new SystemChatMessage($"Additional context from our database:\n{context}"));
        }

        messages.Add(new UserChatMessage($"Wikipedia info: {wikiText}\n\nUser question: {userQuery}"));

        var completion = await _chatClient.CompleteChatAsync(messages);
        return completion.Value.Content[0].Text;
    }

    public async Task<(string? destination, int? days)> ExtractItineraryParams(string userMessage)
{
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage("Extract the destination and number of days from the user's request for a trip plan. Return a JSON with 'destination' and 'days' fields. If not specified, use null for days and a guessed destination or null."),
        new UserChatMessage(userMessage)
    };
    var completion = await _chatClient.CompleteChatAsync(messages);
    var raw = completion.Value.Content[0].Text;
    Console.WriteLine($"ExtractItineraryParams raw JSON: {raw}");

    // Clean the response: remove markdown code fences if present
    var json = ExtractJsonFromResponse(raw);
    Console.WriteLine($"ExtractItineraryParams cleaned JSON: {json}");

    try
    {
        using var doc = JsonDocument.Parse(json);
        var dest = doc.RootElement.TryGetProperty("destination", out var d) ? d.GetString() : null;
        var days = doc.RootElement.TryGetProperty("days", out var dayElem) && dayElem.ValueKind != JsonValueKind.Null ? dayElem.GetInt32() : (int?)null;
        return (dest, days);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"JSON parsing failed: {ex.Message}");
        return (null, null);
    }
}

private string ExtractJsonFromResponse(string raw)
{
    // Try to extract JSON from ```json ... ``` block
    var match = System.Text.RegularExpressions.Regex.Match(raw, @"```json\s*([\s\S]*?)\s*```");
    if (match.Success)
        return match.Groups[1].Value;
    // If no code block, return raw (might be pure JSON)
    return raw;
}
public async Task<string> GenerateItineraryAsync(string prompt)
{
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage("You are a knowledgeable Lebanon travel guide. Provide detailed, engaging itineraries with specific places and activities."),
        new UserChatMessage(prompt)
    };
    var completion = await _chatClient.CompleteChatAsync(messages);
    return completion.Value.Content[0].Text;
}

    public async Task<string> GenerateItineraryAsync(string destination, int days)
{
    var prompt = $@"Create a detailed {days}-day travel itinerary for {destination}, Lebanon. 
The itinerary should be well-organized with clear headings for each day. 
For each day, include:
- Morning activity (specific place with brief description)
- Afternoon activity
- Evening activity (dinner recommendation)
Make each day's activities distinct and varied, covering historical sites, cultural spots, local cuisine, and nature.
Use bullet points for readability.
Keep descriptions concise but informative.

Format the response as follows (use markdown):

**Day 1:**
- Morning: [activity]
- Afternoon: [activity]
- Evening: [activity]

**Day 2:**
... and so on.";

    var messages = new List<ChatMessage>
    {
        new SystemChatMessage("You are a knowledgeable Lebanon travel guide. Provide detailed, engaging itineraries with specific places and activities."),
        new UserChatMessage(prompt)
    };
    var completion = await _chatClient.CompleteChatAsync(messages);
    return completion.Value.Content[0].Text;
}

    // Kept for backward compatibility (used by follow-up detection)
    public async Task<string> FormatAsync(string wikiData, string context = "")
    {
        return await FormatWikiAsync(wikiData, "Tell me about this", context);
    }
}