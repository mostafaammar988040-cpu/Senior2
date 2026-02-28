import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/MyTrips.css";

export default function MyTrips() {

  const [trips, setTrips] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get("/profile/me")
      .then(res => {
        setTrips(res.data.trips || []);
      })
      .catch(err => console.log(err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p className="loading">Loading trips...</p>;

  return (
    <section className="trips-page">

      <h1>✈️ My Smart Trips</h1>
      <p>Your AI generated travel history</p>

      <div className="trips-grid">

        {trips.length === 0 && (
          <p>No trips generated yet.</p>
        )}

        {trips.map(t => (
          <div key={t.id} className="trip-card">

            <h3>{t.tripType}</h3>

            <p>
              📅 {new Date(t.startDate).toLocaleDateString()}
              {" → "}
              {new Date(t.endDate).toLocaleDateString()}
            </p>

            <p>💰 Budget/day: ${t.budgetPerDay}</p>

            <p>👥 {t.travelers}</p>

            <p>🚗 {t.transport}</p>

          </div>
        ))}

      </div>

    </section>
  );
}