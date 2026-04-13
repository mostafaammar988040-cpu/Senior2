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
  console.log(res.data.user);  // <-- add this
  setUser(res.data.user);
})
      .catch(err => console.log(err));
  }, []);

  if (!user) return <p className="loading">Loading Dashboard...</p>;

  return (
    <section className="db-wrap">

      {/* ===== SIDEBAR ===== */}
      <aside className="db-sidebar">
        <div className="db-avatar-ring">
          <img
            className="db-avatar"
            src={user.profileImageUrl || `https://ui-avatars.com/api/?name=${user.firstName}+${user.lastName}&background=0f2027&color=fff&size=110`}
            alt="profile"
          />
        </div>

        <div className="db-name">{user.firstName} {user.lastName}</div>
        <div className="db-email">{user.email}</div>

        {user.bio && <div className="db-bio">{user.bio}</div>}

        <div className="db-divider"></div>

        <div className="db-stat-row">
          <div className="db-stat">
            <div className="db-stat-num">12</div>
            <div className="db-stat-label">Trips</div>
          </div>
          <div className="db-stat">
            <div className="db-stat-num">5</div>
            <div className="db-stat-label">Journeys</div>
          </div>
          <div className="db-stat">
            <div className="db-stat-num">3</div>
            <div className="db-stat-label">Reviews</div>
          </div>
          <div className="db-stat">
            <div className="db-stat-num">8</div>
            <div className="db-stat-label">Saved</div>
          </div>
        </div>

        <div className="db-badge">✦ Explorer Member</div>
      </aside>

      {/* ===== MAIN ===== */}
      <main className="db-main">

        <div className="db-header">
          <div className="db-greeting">Welcome back 👋</div>
          <div className="db-title">My <span>Dashboard</span></div>
        </div>

        <div className="db-cards">

          <div className="db-card trips" onClick={() => window.location.href = "/my-trips"}>
            <div className="db-card-icon">✈️</div>
            <div className="db-card-title">My Trips</div>
            <div className="db-card-desc">View and manage all your planned and past trips across Lebanon.</div>
            <div className="db-card-arrow">→</div>
          </div>

          <div className="db-card journeys" onClick={() => navigate("/profile/journeys")}>
            <div className="db-card-icon">📝</div>
            <div className="db-card-title">My Journeys</div>
            <div className="db-card-desc">Track your personal journey logs and travel memories.</div>
            <div className="db-card-arrow">→</div>
          </div>

          <div className="db-card prefs" onClick={() => navigate("/profile/preferences")}>
            <div className="db-card-icon">❤️</div>
            <div className="db-card-title">Preferences</div>
            <div className="db-card-desc">Set your travel preferences for smarter AI recommendations.</div>
            <div className="db-card-arrow">→</div>
          </div>

          <div className="db-card suggest" onClick={() => navigate("/profile/suggestions")}>
            <div className="db-card-icon">💡</div>
            <div className="db-card-title">Add Suggestion</div>
            <div className="db-card-desc">Know a hidden gem? Share it with the Ahla Bhal Talleh community.</div>
            <div className="db-card-arrow">→</div>
          </div>

        </div>

        <div className="db-banner">
          <div className="db-banner-text">
            <h3>Plan your next adventure with AI ✨</h3>
            <p>Let our smart itinerary builder craft the perfect Lebanon trip for you.</p>
          </div>
          <button className="db-banner-btn" onClick={() => navigate("/SmartItineraryintro")}>
            Start Planning
          </button>
        </div>

      </main>
    </section>
  );
}
