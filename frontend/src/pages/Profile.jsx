import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Profile.css";
import i18n from "i18next";
import { useTranslation } from "react-i18next";

export default function Profile() {
  const { t } = useTranslation();

  const [user, setUser] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [passwordMode, setPasswordMode] = useState(false);
  const [formData, setFormData] = useState({});
  const [passwordData, setPasswordData] = useState({ currentPassword: "", newPassword: "" });

  const navigate = useNavigate();

  useEffect(() => {
    api.get("/profile/me")
      .then(res => setUser(res.data.user))
      .catch(err => console.log(err));
  }, []);

  if (!user) return <p className="loading">{t("profile.loading")}</p>;

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await api.put("/profile/me", formData);
      setUser(res.data.user);
      setEditMode(false);
    } catch (err) {
      console.error(err);
    }
  };

  const handlePasswordSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post("/profile/change-password", passwordData);
      alert(t("profile.passwordSuccess"));
      setPasswordMode(false);
    } catch (err) {
      console.error(err);
      alert(t("profile.passwordError"));
    }
  };

  return (
    <section className="db-wrap">

      {/* SIDEBAR */}
      <aside className="db-sidebar">

        <div className="db-avatar-ring">
          <img
            className="db-avatar"
            src={user.profileImageUrl || `https://ui-avatars.com/api/?name=${user.firstName}+${user.lastName}`}
            alt="profile"
          />
        </div>

        <div className="db-name">{user.firstName} {user.lastName}</div>
        <div className="db-email">{user.email}</div>

        {user.bio && <div className="db-bio">{user.bio}</div>}

       

        <div className="db-badge">{t("profile.badge")}</div>
      </aside>

      {/* MAIN */}
      <main className="db-main">

        {/* LANGUAGE */}
        <div className="profile-section">
          <h3>🌐 {t("profile.language")}</h3>

          <select
            value={i18n.language}
            onChange={(e) => {
              i18n.changeLanguage(e.target.value);
              document.documentElement.dir = e.target.value === "ar" ? "rtl" : "ltr";
            }}
            className="language-select"
          >
            <option value="en">English</option>
            <option value="ar">العربية</option>
            <option value="fr">Français</option>
          </select>
        </div>

        <div className="db-header">
          <div className="db-greeting">{t("profile.welcome")}</div>
          <div className="db-title">
            {t("profile.dashboard")} <span>{t("profile.highlight")}</span>
          </div>
        </div>

        <div className="db-cards">

          <div className="db-card" onClick={() => navigate("/my-trips")}>
            <div className="db-card-title">{t("profile.myTrips")}</div>
            <div className="db-card-desc">{t("profile.myTripsDesc")}</div>
          </div>

          <div className="db-card" onClick={() => navigate("/profile/journeys")}>
            <div className="db-card-title">{t("profile.myJourneys")}</div>
            <div className="db-card-desc">{t("profile.myJourneysDesc")}</div>
          </div>

          <div className="db-card" onClick={() => navigate("/profile/preferences")}>
            <div className="db-card-title">{t("profile.preferences")}</div>
            <div className="db-card-desc">{t("profile.preferencesDesc")}</div>
          </div>

          <div className="db-card" onClick={() => navigate("/profile/favorites")}>
            <div className="db-card-title">{t("profile.favorites")}</div>
            <div className="db-card-desc">{t("profile.favoritesDesc")}</div>
          </div>

          <div className="db-card" onClick={() => navigate("/profile/suggestions")}>
            <div className="db-card-title">{t("profile.suggestion")}</div>
            <div className="db-card-desc">{t("profile.suggestionDesc")}</div>
          </div>

        </div>

        <div className="db-banner">
          <h3>{t("profile.aiTitle")}</h3>
          <p>{t("profile.aiDesc")}</p>

          <button onClick={() => navigate("/SmartItineraryintro")}>
            {t("profile.start")}
          </button>
        </div>

      </main>

    </section>
  );
}