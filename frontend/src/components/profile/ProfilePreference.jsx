import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import api from "../../services/api";
import "../../styles/ProfilePreference.css";

function ProfilePreference() {

  const { t } = useTranslation();

  const [preferences, setPreferences] = useState(null);

  const user = JSON.parse(localStorage.getItem("user") || "null");

  useEffect(() => {
    if (!user) return;

    api.get(`/preferences/${user.id}`)
      .then(res => {
        setPreferences(res.data);
      })
      .catch(() => {
        console.log("No preferences found");
      });

  }, []);

  if (!preferences) {
    return (
      <div style={{ padding: "30px", textAlign: "center" }}>
        <h3>{t("profilePref.emptyTitle")}</h3>
        <p>{t("profilePref.emptyText")}</p>
      </div>
    );
  }

  return (
    <div className="profile-pref-container">
      <div className="pref-wrapper">

        <h2 className="pref-title">{t("profilePref.title")}</h2>

        <div className="pref-grid">

          {/* INTERESTS */}
          <div className="pref-card interest">
            <div className="pref-header">
              <span className="pref-emoji">🎯</span>
              <h3>{t("profilePref.interests")}</h3>
            </div>

            <div className="pref-tags">
              {preferences.interests.map((item, i) => (
                <span key={i} className="pref-tag">
                  {t(`pref.interests.${item}`)}
                </span>
              ))}
            </div>
          </div>

          {/* FOOD */}
          <div className="pref-card food">
            <div className="pref-header">
              <span className="pref-emoji">🍽</span>
              <h3>{t("profilePref.food")}</h3>
            </div>

            <div className="pref-tags">
              {preferences.food.map((item, i) => (
                <span key={i} className="pref-tag">
                  {t(`pref.food.${item}`)}
                </span>
              ))}
            </div>
          </div>

          {/* ACTIVITIES */}
          <div className="pref-card activity">
            <div className="pref-header">
              <span className="pref-emoji">🏔</span>
              <h3>{t("profilePref.activities")}</h3>
            </div>

            <div className="pref-tags">
              {preferences.activities.map((item, i) => (
                <span key={i} className="pref-tag">
                  {t(`pref.activities.${item}`)}
                </span>
              ))}
            </div>
          </div>

        </div>

      </div>
    </div>
  );
}

export default ProfilePreference;