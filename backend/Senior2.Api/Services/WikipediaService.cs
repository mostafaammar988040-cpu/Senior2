using System.Text.Json;

namespace Senior2.Api.Services;

public class WikipediaService
{
    private readonly HttpClient _http;
    private static readonly Dictionary<string, string> LebaneseAliases =
    new Dictionary<string, string>
{
    { "saida", "Sidon" },
    { "bchare", "Bsharri" },
    { "bcharre", "Bsharri" },
    { "baalbeck", "Baalbek" },
    { "baalbek", "Baalbek" },
    { "tyre", "Tyre, Lebanon" },
    { "jbeil", "Byblos" }
};

    public WikipediaService(HttpClient http)
    {
        _http = http;

        _http.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Senior2LebanonAI/1.0 (ammar@example.com)");
    }

    public async Task<string?> GetInfoAsync(string userQuestion)
    {
        var topics = ExtractTopics(userQuestion);

        var summaries = new List<string>();

        foreach (var topic in topics)
        {
            var summary = await FetchSummary(topic);

            if (summary != null)
            {
                summaries.Add(summary);
            }
        }

        if (summaries.Count == 0)
            return null;

        return string.Join("\n\n", summaries);
    }

    private async Task<string?> FetchSummary(string title)
    {
        var url = $"https://en.wikipedia.org/api/rest_v1/page/summary/{Uri.EscapeDataString(title)}";

        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        // 1️⃣ Reject disambiguation pages
        if (doc.RootElement.TryGetProperty("type", out var type))
        {
            if (type.GetString() == "disambiguation")
            {
                return null;
            }
        }

        // 2️⃣ NEW STEP 3 — Validate it's related to Lebanon
        if (doc.RootElement.TryGetProperty("description", out var description))
        {
            var desc = description.GetString()?.ToLower();

            // Validate Lebanon relevance (except when title itself is Lebanon)
            if (title.ToLower() != "lebanon")
            {
                if (doc.RootElement.TryGetProperty("description", out var descElement))
                {
                    var descriptionText = descElement.GetString()?.ToLower();

                    if (descriptionText != null && !descriptionText.Contains("lebanon"))
                    {
                        return null;
                    }
                }
            }

        }

        // 3️⃣ Return summary
        if (doc.RootElement.TryGetProperty("extract", out var extract))
        {
            return extract.GetString();
        }

        return null;

        try
{
    // existing HTTP request
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Wikipedia request failed: {ex.Message}");
    return null;
}
    }


    private List<string> ExtractTopics(string question)
    {
        var lower = question.ToLower();

        // Split on "and"
        var parts = lower.Split(new[] { " and ", "," }, StringSplitOptions.RemoveEmptyEntries);

        var topics = new List<string>();

        foreach (var part in parts)
        {
            var cleaned = part.Trim();

            var wordsToIgnore = new List<string>
        {
            "what", "is", "the", "history", "of",
            "tell", "me", "about", "give", "short"
        };

            var words = cleaned
                .Replace("?", "")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var filtered = words
                .Where(w => !wordsToIgnore.Contains(w.ToLower()))
                .ToList();

            if (filtered.Count > 0)
            {
                var topic = string.Join(" ", filtered);
                topics.Add(topic);
            }
        }

        if (topics.Count == 0)
            topics.Add("Lebanon");

        return topics;
    }

}
