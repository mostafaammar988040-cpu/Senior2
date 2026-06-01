import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect, useRef } from "react";
import api from "../services/api";
import "../styles/navbar.css";
import { FaUserCircle } from "react-icons/fa";
import { useTranslation } from "react-i18next";

export default function Navbar() {
  const { t, i18n } = useTranslation();
  const navigate = useNavigate();

  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const [selectedNotification, setSelectedNotification] = useState(null);

  const firstNotificationLoad = useRef(true);
  const previousUnreadCount = useRef(0);

  const user = JSON.parse(localStorage.getItem("user"));

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
    if (!isLoggedIn) {
      setNotifications([]);
      return;
    }

    const fetchNotifications = async () => {
      try {
        const res = await api.get("/notifications/me");

        const data = res.data;
        setNotifications(data);

        const unreadCount = data.filter(n => !n.isRead).length;

        if (firstNotificationLoad.current) {
          previousUnreadCount.current = unreadCount;
          firstNotificationLoad.current = false;
          return;
        }

        // sound optional: works only if you add public/sounds/notification.mp3
        if (unreadCount > previousUnreadCount.current) {
          const audio = new Audio("/sounds/notification.mp3");
          audio.play().catch(() => {
            console.log("Notification sound blocked until user interacts with page.");
          });
        }

        previousUnreadCount.current = unreadCount;
      } catch (err) {
        console.error("Navbar notifications error:", err);
      }
    };

    fetchNotifications();

    const interval = setInterval(fetchNotifications, 10000);

    return () => clearInterval(interval);
  }, [isLoggedIn]);

  // ==========================
  // READ NOTIFICATION
  // ==========================
  const openNotification = async (notification) => {
    setSelectedNotification(notification);

    if (!notification.isRead) {
      try {
        await api.put(`/notifications/read/${notification.id}`);

        setNotifications(prev =>
          prev.map(n =>
            n.id === notification.id
              ? { ...n, isRead: true }
              : n
          )
        );
      } catch (err) {
        console.error("Read notification error:", err);
      }
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

  // ==========================
  // LANGUAGE SWITCH
  // ==========================
  const changeLanguage = (lng) => {
    i18n.changeLanguage(lng);
    document.documentElement.dir = lng === "ar" ? "rtl" : "ltr";
  };

  return (
    <nav className="navbar">

      {/* LOGO */}
      <h1 className="logo">
        <span className="red">AHLA</span>{" "}
        <span className="green">BHAL</span>{" "}
        <span className="white">TALLEH</span>
      </h1>

      <div className="nav-menu">

        {isLoggedIn && (
          <>
            <Link to="/">{t("navbar.home")}</Link>
            <Link to="/events">{t("navbar.events")}</Link>
            <Link to="/taxis">{t("navbar.taxi")}</Link>
            <Link to="/feed">{t("navbar.feed")}</Link>
            <Link to="/experiences">{t("navbar.experiences")}</Link>
            <Link to="/recommendations">{t("navbar.recommendations")}</Link>
            <Link to="/SmartItineraryintro">{t("navbar.itinerary")}</Link>
            <Link to="/ai-assistant">{t("navbar.ai")}</Link>
          </>
        )}

        {/* NOTIFICATIONS */}
        {isLoggedIn && (
          <div className="notification-wrapper">
            <span
              className="notification-bell"
              onClick={() => setShowNotifications(!showNotifications)}
            >
              🔔
            </span>

            {unreadCount > 0 && (
              <span className="notification-badge">{unreadCount}</span>
            )}

            {showNotifications && (
              <div className="notification-dropdown">
                <h4>{t("notifications.title")}</h4>

                {notifications.length === 0 && (
                  <p>{t("notifications.empty")}</p>
                )}

                {notifications.map(n => (
                  <div
                    key={n.id}
                    className={`notification-dropdown-item ${n.isRead ? "read" : "unread"}`}
                    onClick={() => openNotification(n)}
                  >
                    <p>{n.message}</p>
                    <small>{new Date(n.createdAt).toLocaleString()}</small>
                  </div>
                ))}
              </div>
            )}
          </div>
        )}

        {isLoggedIn ? (
          <span onClick={handleLogout} className="logout">
            {t("navbar.logout")}
          </span>
        ) : (
          <Link to="/login">{t("navbar.login")}</Link>
        )}

        {isLoggedIn && (
          <Link to="/profile" className="nav-profile">
            {user?.profileImageUrl ? (
              <img
                src={user.profileImageUrl}
                alt="Profile"
                className="profile-img"
              />
            ) : (
              <FaUserCircle className="profile-icon" />
            )}
          </Link>
        )}

      </div>

      {/* MODAL */}
      {selectedNotification && (
        <div className="notification-modal-overlay">
          <div className="notification-modal">
            <h3>{t("notifications.title")}</h3>

            <p>{selectedNotification.message}</p>

            <small>
              {new Date(selectedNotification.createdAt).toLocaleString()}
            </small>

            <button
              className="close-modal"
              onClick={() => setSelectedNotification(null)}
            >
              {t("adminSupport.close")}
            </button>
          </div>
        </div>
      )}
    </nav>
  );
}