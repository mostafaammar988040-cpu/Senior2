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

   <nav className="navbar">

  {/* LEFT → LOGO */}
  <div className="nav-left">
    <h1>
      <span style={{ color: "#d62828" }}>AHLA</span>{" "}
      <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
      <span style={{ color: "#f9f7f7" }}>TALLEH</span>
    </h1>
  </div>

  {/* CENTER → LINKS */}
  <div className="nav-center">
    <Link to="/">Home</Link>
    <Link to="/events">Events</Link>
    <Link to="/ai-assistant">AI Assistant</Link>
    <Link to="/SmartItineraryintro">Smart Itinerary</Link>
    <Link to="/taxis">Taxi Services</Link>
    <Link to="/experiences">Experiences</Link>
    <Link to="/feed">Feed</Link>
  </div>

  {/* RIGHT → LANGUAGE + USER */}
  <div className="nav-right">

    {/* 🌐 Language */}

    {isLoggedIn && <Link to="/profile">Profile</Link>}
    {isLoggedIn && <Link to="/recommendations">Recommendations</Link>}

    {/* 🔔 Notifications */}
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
              <p>No notifications</p>
            )}
            {notifications.map(n => (
              <div key={n.id} onClick={() => openNotification(n)}>
                <p>{n.message}</p>
              </div>
            ))}
          </div>
        )}
      </div>
    )}

    {/* LOGIN / LOGOUT */}
    {isLoggedIn ? (
      <span onClick={handleLogout} className="logout">
        Logout
      </span>
    ) : (
      <Link to="/login">Login</Link>
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