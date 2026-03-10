import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/MyTrips.css";

export default function MyTrips() {

  const [trips, setTrips] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedTrip, setSelectedTrip] = useState(null);

  useEffect(() => {
    fetchTrips();
  }, []);

  const fetchTrips = async () => {

    try {

      const res = await api.get("/profile/me");

      setTrips(res.data.trips || []);

    } catch (error) {

      console.error("Error fetching trips:", error);

    } finally {

      setLoading(false);

    }

  };

  const cancelTrip = async (tripId) => {

    const confirm = window.confirm("Are you sure you want to cancel this trip?");

    if (!confirm) return;

    try {

      await api.put(`/SmartItinerary/cancel/${tripId}`);

      // update UI without refresh
      setTrips(prev =>
        prev.map(t =>
          t.id === tripId ? { ...t, status: "Cancelled" } : t
        )
      );

      setSelectedTrip(prev => ({
        ...prev,
        status: "Cancelled"
      }));

    } catch (err) {

      console.error(err);
      alert("Failed to cancel trip");

    }

  };

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
    className={`trip-card ${t.status === "Cancelled" ? "cancelled" : ""}`}
    onClick={() => setSelectedTrip(t)}
  >

    <h3>{t.tripType}</h3>

    {t.status === "Cancelled" && (
      <div className="trip-overlay">
        Cancelled
      </div>
    )}

    {t.status === "Completed" && (
      <div className="trip-overlay completed">
        Completed
      </div>
    )}

  </div>

))}
      </div>


      {/* MODAL */}

      {selectedTrip && (

        <div
          className="trip-modal-overlay"
          onClick={() => setSelectedTrip(null)}
        >

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

            <h2>{selectedTrip.tripType} Trip</h2>

            <p>
              📅 {new Date(selectedTrip.startDate).toLocaleDateString()}
              {" → "}
              {new Date(selectedTrip.endDate).toLocaleDateString()}
            </p>

            <p>💰 Budget/day: ${selectedTrip.budgetPerDay}</p>

            <p>👥 Travelers: {selectedTrip.travelers}</p>

            <p>🚗 Transport: {selectedTrip.transport}</p>
            {/* TRIP ITINERARY */}

{selectedTrip.itineraryJson && (

  <div className="trip-itinerary">

    <h3>🗺️ Trip Itinerary</h3>

    {JSON.parse(selectedTrip.itineraryJson).map(day => (

  <div key={day.day} className="trip-day">

    <h4>Day {day.day} — {day.region}</h4>

    {/* ACTIVITIES */}
    {day.activities?.map(place => (

      <p key={place.id}>
        • {place.name} — {place.location}
      </p>

    ))}

    {/* RESTAURANT */}
    {day.restaurant && (
      <p>
        🍽 Restaurant: {day.restaurant.name}
      </p>
    )}

  </div>

))}

  </div>

)}

            <p>
              Created:
              {" "}
              {new Date(selectedTrip.createdAt).toLocaleDateString()}
            </p>

            {/* STATUS */}

            <p className="trip-status">
              Status: {selectedTrip.status || "Active"}
            </p>

            {/* CANCEL BUTTON */}

            {selectedTrip.status !== "Cancelled" && (
              <button
                className="cancel-trip-btn"
                onClick={() => cancelTrip(selectedTrip.id)}
              >
                Cancel Trip
              </button>
            )}

          </div>

        </div>

      )}

    </section>
  );
}