import { useEffect, useState } from "react";
import api from "../../services/api";
import "../../styles/ProfilePreference.css";
function ProfilePreference() {

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
        <h3>No preferences yet 😕</h3>
        <p>Go set your preferences to get better recommendations!</p>
      </div>
    );
  }

  return (
  <div className="profile-pref-container">

    <div className="pref-panel">

      <h2>Your Preferences</h2>

      {/* INTERESTS */}
      <div className="pref-section">
        <h3>🎯 Interests</h3>
        <div className="pref-tags">
          {preferences.interests.map((item, i) => (
            <span key={i} className="pref-tag">{item}</span>
          ))}
        </div>
      </div>

      {/* FOOD */}
      <div className="pref-section">
        <h3>🍽 Food</h3>
        <div className="pref-tags">
          {preferences.food.map((item, i) => (
            <span key={i} className="pref-tag">{item}</span>
          ))}
        </div>
      </div>

      {/* ACTIVITIES */}
      <div className="pref-section">
        <h3>🏔 Activities</h3>
        <div className="pref-tags">
          {preferences.activities.map((item, i) => (
            <span key={i} className="pref-tag">{item}</span>
          ))}
        </div>
      </div>

    </div>

  </div>
);
}

export default ProfilePreference;