import { useEffect, useState } from "react";
import api from "../services/api";

function Notifications() {
  const [notifications, setNotifications] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (!token) {
      console.log("No token found");
      return;
    }

    api.get("/notifications/me")
      .then(res => {
        setNotifications(res.data);
      })
      .catch(err => {
        console.error("Notifications error:", err);
      });
  }, []);

  const markAsRead = async (id) => {
    try {
      await api.put(`/notifications/read/${id}`);

      setNotifications(prev =>
        prev.map(n =>
          n.id === id ? { ...n, isRead: true } : n
        )
      );
    } catch (err) {
      console.error("Mark as read error:", err);
    }
  };

  return (
    <div className="notifications-page">

      <h2>Notifications</h2>

      {notifications.length === 0 && (
        <p>No notifications yet.</p>
      )}

      {notifications.map(n => (
        <div
          key={n.id}
          className={`notification ${n.isRead ? "read" : ""}`}
          onClick={() => markAsRead(n.id)}
        >
          <p>{n.message}</p>

          <small>
            {new Date(n.createdAt).toLocaleString()}
          </small>
        </div>
      ))}

    </div>
  );
}

export default Notifications;