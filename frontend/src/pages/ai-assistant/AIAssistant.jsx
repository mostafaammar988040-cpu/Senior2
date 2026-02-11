import { IoSend } from "react-icons/io5";
import "./AIAssistant.css";
import { useState } from "react";

function AIAssistant(){
    const [input, setInput] = useState("")
const [messages, setMessages] = useState([])
const handleSend = async () => {

  // 1️⃣ Validate input
  if (!input.trim()) {
    alert("Please ask a question");
    return;
  }

  // 2️⃣ Create user message object
  const userMessage = {
    sender: "user",
    text: input
  };

  // 3️⃣ Add user message to chat
  setMessages((prevMessages) => [
    ...prevMessages,
    userMessage
  ]);

  // 4️⃣ Clear input field
  setInput("");

  try {

    // 5️⃣ Simulate AI response (temporary)
    const aiMessage = {
      sender: "ai",
      text: "Thinking..."
    };

    setMessages((prevMessages) => [
      ...prevMessages,
      aiMessage
    ]);

  } catch (error) {
    console.error("Error sending message:", error);
  }
};

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
          </h1>    </div>

    <h2 className="ai-subtitle">
      🤖 Your Lebanon AI Travel Assistant
    </h2>
  </div>

  <div className="chat-messages">
{messages.map((msg, index) => (
    <div 
      key={index} 
      className={msg.sender === "user" ? "user-message" : "ai-message"}
    >
      {msg.text}
    </div>
  ))}  </div>

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
