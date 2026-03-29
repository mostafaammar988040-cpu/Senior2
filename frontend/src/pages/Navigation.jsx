import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../services/api";
import "../styles/Homepage.css";

export default function Navbar() {

  const navigate = useNavigate();

  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const [selectedNotification, setSelectedNotification] = useState(null);

  // ==========================
  // LOGIN STATE
  // ==========================

  useEffect(() => {

    const checkLogin = () => {
      setIsLoggedIn(!!localStorage.getItem("token"));
    };

    checkLogin();

    window.addEventListener("storage", checkLogin);
    window.addEventListener("loginChange", checkLogin);

    return () => {
      window.removeEventListener("storage", checkLogin);
      window.removeEventListener("loginChange", checkLogin);
    };

  }, []);

  // ==========================
  // LOAD NOTIFICATIONS
  // ==========================

  useEffect(() => {

    if (!isLoggedIn) return;

    const user = JSON.parse(localStorage.getItem("user"));

    api.get(`/notifications/${user.id}`)
      .then(res => {
        setNotifications(res.data);
      });

  }, [isLoggedIn]);

  // ==========================
  // READ NOTIFICATION
  // ==========================

  const openNotification = async (notification) => {

    setSelectedNotification(notification);

    if (!notification.isRead) {

      await api.put(`/notifications/read/${notification.id}`);

      setNotifications(prev =>
        prev.map(n =>
          n.id === notification.id
            ? { ...n, isRead: true }
            : n
        )
      );

    }

  };

  const unreadCount = notifications.filter(n => !n.isRead).length;

  // ==========================
  // LOGOUT
  // ==========================

  const handleLogout = () => {

    localStorage.removeItem("token");
    localStorage.removeItem("user");

    window.dispatchEvent(new Event("loginChange"));

    navigate("/");

  };

  return (

    <nav>

      <h1>
        <span style={{ color: "#d62828" }}>AHLA</span>{" "}
        <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
        <span style={{ color: "#f9f7f7" }}>TALLEH</span>
      </h1>

      <div>

        <Link to="/" style={{ marginLeft: "25px" }}>Home</Link>
        <Link to="/events" style={{ marginLeft: "25px" }}>Events</Link>
        <Link to="/ai-assistant" style={{ marginLeft: "25px" }}>AI Assistant</Link>
        <Link to="/SmartItineraryintro" style={{ marginLeft: "25px" }}>Smart Itinerary</Link>
        <Link to="/taxis" style={{ marginLeft: "25px" }}>Taxi Services</Link>
        <Link to="/experiences" style={{ marginLeft: "25px" }}>Experiences</Link>
        <Link to="/feed" style={{ marginLeft: "25px" }}>Feed</Link>
        <Link to="/create-journey-entry" style={{ marginLeft: "25px" }}>Create Journey Entry</Link>
        

        {isLoggedIn && (
          <Link to="/profile" style={{ marginLeft: "25px" }}>
            Profile
          </Link>
        )}

        {isLoggedIn && (
          <Link to="/recommendations" style={{ marginLeft: "25px" }}>
            Recommendations
          </Link>
        )}

        {/* 🔔 NOTIFICATIONS */}

        {isLoggedIn && (

          <div className="notification-wrapper">

            <span
              className="notification-bell"
              onClick={() => setShowNotifications(!showNotifications)}
            >
              🔔
            </span>

            {unreadCount > 0 && (
              <span className="notification-badge">
                {unreadCount}
              </span>
            )}

            {showNotifications && (

              <div className="notification-dropdown">

                <h4>Notifications</h4>

                {notifications.length === 0 && (
                  <p className="no-notifications">No notifications</p>
                )}

                {notifications.map(n => (

                  <div
                    key={n.id}
                    className={`notification-item ${n.isRead ? "read" : ""}`}
                    onClick={() => openNotification(n)}
                  >

                    <p>{n.message}</p>

                    <small>
                      {new Date(n.createdAt).toLocaleString()}
                    </small>

                  </div>

                ))}

              </div>

            )}

          </div>

        )}

        {/* LOGOUT */}

        {isLoggedIn ? (
          <span
            onClick={handleLogout}
            style={{
              cursor: "pointer",
              marginLeft: "25px",
              fontWeight: "600"
            }}
          >
            Logout
          </span>
        ) : (
          <Link to="/login" style={{ marginLeft: "25px" }}>
            Login
          </Link>
        )}

      </div>

      {/* ==========================
         NOTIFICATION MODAL
      ========================== */}

      {selectedNotification && (

        <div className="notification-modal-overlay">

          <div className="notification-modal">

            <h3>Notification</h3>

            <p>{selectedNotification.message}</p>

            <small>
              {new Date(selectedNotification.createdAt).toLocaleString()}
            </small>

            <button
              className="close-modal"
              onClick={() => setSelectedNotification(null)}
            >
              Close
            </button>

          </div>

        </div>

      )}

    </nav>
  );

}