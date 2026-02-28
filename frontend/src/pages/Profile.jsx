import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Profile.css";

export default function Profile() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    api.get("/profile/me")
      .then(res => {
        setUser(res.data.user);
      })
      .catch(err => console.log(err));
  }, []);

  if (!user) return <p className="loading">Loading Dashboard...</p>;

  return (
    <section className="dashboard">

      {/* ===== SIDEBAR ===== */}
      <aside className="sidebar">
        <img
          src={user.profileImageUrl || "/default-avatar.png"}
          alt="profile"
          className="profile-img"
        />

        <h2>{user.firstName} {user.lastName}</h2>
        <p className="email">{user.email}</p>

        {user.bio && <p className="bio">{user.bio}</p>}
      </aside>

      {/* ===== MAIN DASHBOARD ===== */}
      <main className="main-content">

        <h1 className="dashboard-title">My Dashboard</h1>

        <div className="dashboard-buttons">

          <button
    className="dashboard-btn"
    onClick={() => window.location.href = "/my-trips"}
  >
            ✈️ My Trips
          </button>

          <button
            className="dash-btn"
            onClick={() => navigate("/profile/journeys")}
          >
            📝 My Journeys
          </button>

          <button
            className="dash-btn"
            onClick={() => navigate("/profile/preferences")}
          >
            ❤️ Preferences
          </button>

          <button
            className="dash-btn"
            onClick={() => navigate("/profile/suggestions")}
          >
            💡 Add Suggestion
          </button>

        </div>

      </main>
    </section>
  );
}