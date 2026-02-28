import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/Profile.css";

export default function Profile() {
  const [user, setUser] = useState(null);
  const [preferences, setPreferences] = useState(null);
  const [trips, setTrips] = useState([]);
  const [journeys, setJourneys] = useState([]);

  useEffect(() => {
    api.get("/profile/me")
      .then(res => {
        setUser(res.data.user);
        setPreferences(res.data.preferences);
        setTrips(res.data.trips || []);
        setJourneys(res.data.journeys || []);
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

        <div className="stats">
          <div className="stat-card">
            <h3>{trips.length}</h3>
            <p>Trips</p>
          </div>

          <div className="stat-card">
            <h3>{journeys.length}</h3>
            <p>Journeys</p>
          </div>
        </div>
      </aside>

      {/* ===== MAIN CONTENT ===== */}
      <main className="main-content">

        {/* ===== TRIPS ===== */}
        <section className="section">
          <h2>✈️ Smart Trips</h2>

          <div className="cards-grid">
            {trips.length === 0 && <p>No trips yet.</p>}

            {trips.map(t => (
              <div key={t.id} className="card">
                <h4>{t.tripType}</h4>
                <p>
                  {new Date(t.startDate).toLocaleDateString()} →
                  {new Date(t.endDate).toLocaleDateString()}
                </p>
                <p>Budget/day: ${t.budgetPerDay}</p>
              </div>
            ))}
          </div>
        </section>

        {/* ===== JOURNEY TIMELINE ===== */}
        <section className="section">
          <h2>📝 Journey Timeline</h2>

          <div className="timeline">
            {journeys.length === 0 && <p>No journeys yet.</p>}

            {journeys.map(j => (
              <div key={j.id} className="timeline-item">
                <h4>{j.title}</h4>
                <p>{j.content}</p>
                <small>
                  {new Date(j.createdAt).toLocaleString()}
                </small>
              </div>
            ))}
          </div>
        </section>

        {/* ===== PREFERENCES ===== */}
        <section className="section">
          <h2>❤️ Preferences</h2>

          <div className="card">
            {preferences ? (
              <pre>{preferences.preferencesJson}</pre>
            ) : (
              <p>No preferences saved.</p>
            )}
          </div>
        </section>

      </main>
    </section>
  );
}