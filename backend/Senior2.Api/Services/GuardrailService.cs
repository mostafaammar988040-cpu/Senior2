using System.Linq;

namespace Senior2.Api.Services;

public class GuardrailService
{
    public bool IsObviouslyOutOfScope(string message)
    {
        var lower = message.ToLower();

        var allowedKeywords = new[]
        {
            // Lebanon locations
            "lebanon","beirut","byblos","jbeil","baalbek","baalbeck","tyre","sidon",
            "batroun","anjar","jeita","cedars","bekaa","koura",

            // tourism keywords
            "restaurant","hotel","guesthouse","cafe","coffee","bar",
            "beach","ski","mountain","hiking","trail","waterfall",

            // travel keywords
            "trip","travel","visit","tour","place","where","recommend",

            // history
            "history","tell me about","what is","who built","when"
        };

        // If ANY allowed keyword exists → allow
        if (allowedKeywords.Any(k => lower.Contains(k)))
            return false;

        return true;
    }
}