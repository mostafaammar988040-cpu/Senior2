using System.Text;
using System.Text.Json;

namespace Senior2.Api.Services;

public class LLMService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public LLMService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> FormatAsync(string text)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        var model = _config["OpenAI:Model"];

        var systemPrompt = @"
You are a travel assistant for Lebanon.

Your job is to help users explore Lebanon by explaining:
- historical sites
- cities
- cultural landmarks
- tourist attractions

Keep answers:
- clear
- friendly
- easy to read
- about 2–4 sentences

If the information is about Lebanon history or tourism, explain it nicely for travelers.
";

        var requestBody = new
        {
            model = model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = text }
            }
        };

        var requestJson = JsonSerializer.Serialize(requestBody);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.openai.com/v1/chat/completions"
        );

        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return "Sorry, I couldn't process that information right now.";
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseContent);

        var reply = doc
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return reply ?? "I couldn't generate a response.";
    }
}