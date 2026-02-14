import "./AIAssistant.css";

function ChatMessage({ message }) {
  const isUser = message.sender === "user";

  return (
    <div className={isUser ? "user-message" : "ai-message"}>
      {message.text}
    </div>
  );
}

export default ChatMessage;
