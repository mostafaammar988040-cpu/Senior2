import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/AdminSupport.css";

function AdminSupport() {
  const [messages, setMessages] = useState([]);
  const [selected, setSelected] = useState(null);
  const [reply, setReply] = useState("");

  useEffect(() => {
    fetchMessages();
  }, []);

  const fetchMessages = async () => {
    const res = await api.get("/support");
    setMessages(res.data);
  };

  const sendReply = async () => {
    try {
await api.post(`/support/reply?id=${selected.id}`, {
  message: reply
});
      // 🔥 update UI instantly
      setMessages(prev =>
        prev.map(r =>
          r.id === selected.id ? { ...r, isReplied: true } : r
        )
      );

      setReply("");
      setSelected(null);

      alert("Reply sent ✅");
    } catch (err) {
      console.error(err);
      alert("Failed to send reply ❌");
    }
  };

  return (
    <div className="admin-support-page">

      <h1 className="title">📩 Users Support</h1>

      <div className="support-table">

        {/* HEADER */}
        <div className="table-header">
          <span>Name</span>
          <span>Email</span>
          <span>Subject</span>
          <span>Date</span>
        </div>

        {/* ROWS */}
        {messages.map(msg => (
          <div
            key={msg.id}
            className={`table-row ${msg.isReplied ? "replied" : ""}`}
            onClick={() => setSelected(msg)}
          >
            <span>{msg.name}</span>
            <span>{msg.email}</span>

            <span>
              {msg.subject}
              {msg.isReplied && (
                <span className="status-badge">Replied</span>
              )}
            </span>

            <span>
              {new Date(msg.createdAt).toLocaleDateString()}
            </span>
          </div>
        ))}

      </div>

      {/* MODAL */}
      {selected && (
        <div className="modal">
          <div className="modal-box">

            <h2>{selected.subject}</h2>

            <p><b>Name:</b> {selected.name}</p>
            <p><b>Email:</b> {selected.email}</p>
            <p><b>Category:</b> {selected.category}</p>

            <div className="message-box">
              {selected.message}
            </div>

            {/* 🔥 REPLY INPUT */}
            <textarea
              placeholder="Write your reply..."
              value={reply}
              onChange={(e) => setReply(e.target.value)}
              className="reply-box"
            />

            <button onClick={sendReply}>Send Reply</button>
            <button onClick={() => setSelected(null)}>Close</button>

          </div>
        </div>
      )}

    </div>
  );
}

export default AdminSupport;