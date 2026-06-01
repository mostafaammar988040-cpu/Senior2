using System.Collections.Concurrent;
using OpenAI.Chat;

namespace Senior2.Api.Services;

public class ConversationMemoryService
{
    private readonly ConcurrentDictionary<string, List<ChatMessage>> _sessions = new();

    public void AddMessage(string sessionId, string role, string content)
    {
        var history = _sessions.GetOrAdd(sessionId, _ => new List<ChatMessage>());
        
        if (role == "user")
            history.Add(new UserChatMessage(content));
        else
            history.Add(new AssistantChatMessage(content));

        // Keep only last 10 messages to avoid token limits
        if (history.Count > 10)
            history.RemoveAt(0);
    }

    public List<ChatMessage> GetHistory(string sessionId)
    {
        return _sessions.TryGetValue(sessionId, out var history) 
            ? new List<ChatMessage>(history) 
            : new List<ChatMessage>();
    }

    public void ClearSession(string sessionId)
    {
        _sessions.TryRemove(sessionId, out _);
    }
}