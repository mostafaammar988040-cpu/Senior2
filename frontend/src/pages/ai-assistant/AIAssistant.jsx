import { IoSend } from "react-icons/io5";
import "./AIAssistant.css";
import { useState, useEffect, useRef } from "react";
import ChatMessage from "./ChatMessage";
import api from "../../services/api";
import { useTranslation } from "react-i18next";

function AIAssistant() {
  const { t } = useTranslation(); 

  const [input, setInput] = useState("");

  const [messages, setMessages] = useState([
    {
      sender: "ai",
      text: t("ai.welcome"), 
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

    const thinkingMessage = {
      sender: "ai",
      text: t("ai.thinking"), 
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
        updated.pop();
        updated.push({
          sender: "ai",
          text: data.reply,
        });
        return updated;
      });
    } catch (err) {
      console.error(err);

      setMessages((prev) => {
        const updated = [...prev];
        updated.pop();
        updated.push({
          sender: "ai",
          text: t("ai.error"), 
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
            🤖 {t("ai.subtitle")} 
          </div>
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
              placeholder={t("ai.placeholder")} 
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