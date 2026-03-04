import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/MyTrips.css";

export default function MyTrips() {

  const [trips, setTrips] = useState([]);
  const [loading, setLoading] = useState(true);

  const [selectedTrip, setSelectedTrip] = useState(null);

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
          <div
            key={t.id}
            className="trip-card"
            onClick={() => setSelectedTrip(t)}
          >
            <h3>{t.tripType}</h3>
          </div>
        ))}

      </div>


      {/* POPUP MODAL */}

      {selectedTrip && (
        <div className="trip-modal-overlay" onClick={() => setSelectedTrip(null)}>

          <div
            className="trip-modal"
            onClick={(e) => e.stopPropagation()}
          >

            <button
              className="close-btn"
              onClick={() => setSelectedTrip(null)}
            >
              ✕
            </button>

            <h2>{selectedTrip.tripType}</h2>

            <p>
              📅 {new Date(selectedTrip.startDate).toLocaleDateString()}
              {" → "}
              {new Date(selectedTrip.endDate).toLocaleDateString()}
            </p>

            <p>💰 Budget/day: ${selectedTrip.budgetPerDay}</p>

            <p>👥 {selectedTrip.travelers}</p>

            <p>🚗 {selectedTrip.transport}</p>

          </div>

        </div>
      )}

    </section>
  );
}