import { IoSend } from "react-icons/io5";
import "./AIAssistant.css";
import { useState, useEffect, useRef } from "react";
import ChatMessage from "./ChatMessage";
import api from '../../services/api';

function AIAssistant() {
  const [input, setInput] = useState("");
  const [messages, setMessages] = useState([]);
  // Generate or retrieve a persistent session ID
const [sessionId] = useState(() => {
  let id = localStorage.getItem('chatSessionId');
  if (!id) {
    id = 'session_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    localStorage.setItem('chatSessionId', id);
  }
  return id;
});

  const bottomRef = useRef(null);

  const handleSend = async () => {
    if (!input.trim()) {
      alert("Please ask a question");
      return;
    }

    const userMessage = {
      sender: "user",
      text: input
    };


    setInput("");

    // Add temporary thinking message
    const thinkingMessage = {
      sender: "ai",
      text: "Thinking..."
    };

    setMessages((prev) => [...prev, userMessage, thinkingMessage]);

    try {
      const response = await api.post('/Chat', { 
  message: userMessage.text,
  sessionId: sessionId 
});
      const data = response.data;
      

      // Replace "Thinking..." with real reply
      setMessages((prev) => {
        const updated = [...prev];
        updated.pop(); // remove Thinking...
        updated.push({
          sender: "ai",
          text: data.reply
        });
        return updated;
      });

    } catch (error) {
      console.error("Error sending message:", error);

      setMessages((prev) => {
        const updated = [...prev];
        updated.pop();
        updated.push({
          sender: "ai",
          text: "Something went wrong. Please try again."
        });
        return updated;
      });
    }
  };

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  return (
    <div className="chat">
      <div className="chat-wrapper">

        <div className="chat-header">
          <div className="logo">
            <img src="/images/cedar.png" alt="Cedar Icon" />
            <h1>
              <span style={{ color: "#d62828" }}>AHLA</span>{" "}
              <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
              <span style={{ color: "#000000" }}>TALLEH</span>
            </h1>
          </div>

          <h2 className="ai-subtitle">
            🤖 Your Lebanon AI Travel Assistant
          </h2>
        </div>

        <div className="chat-messages">
          {messages.map((msg, index) => (
            <ChatMessage key={index} message={msg} />
          ))}
          <div ref={bottomRef} />
        </div>

        <div className="chat-input-area">
          <div className="input-card">
            <input
              type="text"
              placeholder="Ask about Lebanon..."
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  handleSend();
                }
              }}
            />
            <button onClick={handleSend}>
              <IoSend size={18} />
            </button>
          </div>
        </div>

      </div>
    </div>
  );
}

export default AIAssistant;
