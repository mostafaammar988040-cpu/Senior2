import { IoSend } from "react-icons/io5";
import "./AIAssistant.css";
import { useState, useEffect, useRef } from "react";
import ChatMessage from "./ChatMessage";
import api from "../../services/api";

function AIAssistant() {
  const [input, setInput] = useState("");

  // ✅ initialize with default message (fixes warnings)
  const [messages, setMessages] = useState([
    {
      sender: "ai",
      text: "Hi there! Ready to explore Lebanon? 😊",
    },
  ]);

  const [sessionId] = useState(() => {
    let id = localStorage.getItem("chatSessionId");
    if (!id) {
      id =
        "session_" +
        Date.now() +
        "_" +
        Math.random().toString(36).substr(2, 9);
      localStorage.setItem("chatSessionId", id);
    }
    return id;
  });

  const bottomRef = useRef(null);

  const handleSend = async () => {
    if (!input.trim()) return;

    const userMessage = {
      sender: "user",
      text: input,
    };

    setInput("");

    // temporary thinking message
    const thinkingMessage = {
      sender: "ai",
      text: "Thinking...",
    };

    setMessages((prev) => [...prev, userMessage, thinkingMessage]);

    try {
      const response = await api.post("/Chat", {
        message: userMessage.text,
        sessionId: sessionId,
      });

      const data = response.data;

      setMessages((prev) => {
        const updated = [...prev];
        updated.pop(); // remove thinking
        updated.push({
          sender: "ai",
          text: data.reply,
        });
        return updated;
      });
    } catch (err) {
      // ✅ use err → no ESLint warning
      console.error(err);

      setMessages((prev) => {
        const updated = [...prev];
        updated.pop();
        updated.push({
          sender: "ai",
          text: "Something went wrong. Please try again.",
        });
        return updated;
      });
    }
  };

  useEffect(() => {
    if (bottomRef.current) {
      bottomRef.current.scrollIntoView({ behavior: "smooth" });
    }
  }, [messages]);

  return (
    <div className="chat">
      <div className="chat-wrapper">

        {/* HEADER */}
        <div className="chat-header">
          <div className="logo">
            <img src="/images/cedar.png" alt="Cedar" />
            <h1>
              <span style={{ color: "#d62828" }}>AHLA</span>{" "}
              <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
              <span style={{ color: "#000" }}>TALLEH</span>
            </h1>
          </div>

          <div className="ai-subtitle">
            🤖 Your Lebanon AI Travel Assistant
          </div>
        </div>

        {/* MESSAGES */}
        <div className="chat-messages">
          {messages.map((msg, index) => (
            <ChatMessage key={index} message={msg} />
          ))}
          <div ref={bottomRef} />
        </div>

        {/* INPUT */}
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