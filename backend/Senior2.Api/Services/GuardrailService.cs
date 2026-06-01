using System.Text.RegularExpressions;

namespace Senior2.Api.Services;

public class GuardrailService
{
    
    private readonly HashSet<string> _lebanonKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "lebanon", "beirut", "byblos", "baalbek", "tyre", "sidon",
        "cedars", "jeita", "batroun", "jbeil", "anjar", "koura",
        "bekaa", "mount lebanon", "north lebanon", "south lebanon",

        "saifi", "saifi village", "hamra", "gemmayze", "mar mikhael",

        "restaurant", "hotel", "guesthouse", "resort",
        "beach", "ski", "historical", "trip", "itinerary",
        "travel", "tour", "visit", "place",

        "food", "cuisine", "wine", "festival", "event",
        "culture", "nightlife", "shopping", "souk",

        "mountain", "valley", "waterfall", "cave",

        "lebanese", "mezze", "tabbouleh", "kibbeh", "shawarma",

        "transport", "taxi", "car rental", "safety", "covid"
    };

  
    private readonly HashSet<string> _otherCountries = new(StringComparer.OrdinalIgnoreCase)
    {
        "france", "paris", "germany", "berlin", "usa", "united states", "america", "new york",
        "uk", "london", "england", "britain", "spain", "madrid", "italy", "rome",
        "china", "beijing", "japan", "tokyo", "russia", "moscow",
        "india", "australia", "canada", "brazil", "mexico",
        "egypt", "cairo", "turkey", "istanbul", "greece", "athens"
    };

  
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

       
        if (_alwaysAllowed.Any(p => lower.Contains(p)))
            return false;

      
        bool containsOtherCountry = _otherCountries.Any(country =>
            Regex.IsMatch(lower, $@"\b{Regex.Escape(country)}\b")
        );

        bool containsLebanonKeyword = _lebanonKeywords.Any(keyword =>
     Regex.IsMatch(lower, $@"\b{Regex.Escape(keyword)}\b")
 );
       
        if (hasHistory)
        {
            return containsOtherCountry && !containsLebanonKeyword;
        }

     
        bool looksLikePlaceQuery =
            lower.Contains(" in ") ||
            lower.Contains(" near ") ||
            lower.Contains(" around ") ||
            lower.Contains("coffee") ||
            lower.Contains("cafe") ||
            lower.Contains("restaurant") ||
            lower.Contains("hotel") ||
            lower.Contains("bar") ||
            lower.Contains("club");

        if (looksLikePlaceQuery)
            return false;

      
        if (containsOtherCountry && !containsLebanonKeyword)
            return true;

       
        return false;
    }
}