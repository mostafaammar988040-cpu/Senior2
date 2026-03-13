import { useEffect, useState } from "react";
import api from "../services/api";

function Notifications() {

  const [notifications, setNotifications] = useState([]);

  const user = JSON.parse(localStorage.getItem("user"));

  useEffect(() => {

    api.get(`/notifications/${user.id}`)
      .then(res => {
        setNotifications(res.data);
      });

  }, []);

  return (

    <div className="notifications-page">

      <h2>Notifications</h2>

      {notifications.map(n => (

        <div
          key={n.id}
          className={`notification ${n.isRead ? "read" : ""}`}
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