import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../services/api";
import "../styles/navbar.css";
import { FaUserCircle } from "react-icons/fa";
import { useTranslation } from "react-i18next";

export default function Navbar() {

  const { t, i18n } = useTranslation(); // 🔥 added i18n for language switch
  const navigate = useNavigate();

  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const [selectedNotification, setSelectedNotification] = useState(null);

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

    if (!isLoggedIn) return;

    const user = JSON.parse(localStorage.getItem("user"));

    api.get(`/notifications/${user.id}`)
      .then(res => setNotifications(res.data));

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

  // ==========================
  // LANGUAGE SWITCH (🔥 NEW)
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
              <span className="notification-badge">{unreadCount}</span>
            )}

            {showNotifications && (
              <div className="notification-dropdown">
                <h4>{t("notifications.title")}</h4>

                {notifications.length === 0 && (
                  <p>{t("notifications.empty")}</p>
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