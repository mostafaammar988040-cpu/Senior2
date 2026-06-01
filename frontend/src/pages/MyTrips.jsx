import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import api from "../services/api";
import "../styles/MyTrips.css";
import React from "react";

export default function MyTrips() {

  const { t } = useTranslation();

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

    const confirmCancel = window.confirm(t("trips.confirmCancel"));
    if (!confirmCancel) return;

    try {

      await api.put(`/SmartItinerary/cancel/${tripId}`);

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
      alert(t("trips.cancelError"));
    }

  };

  if (loading) return <p className="loading">{t("trips.loading")}</p>;

  return (
    <section className="trips-page">

      {/* HEADER */}
      <div className="trips-header">
        <h1>✈️ {t("trips.title")}</h1>
        <p>{t("trips.subtitle")}</p>
      </div>

      {/* GRID */}
      <div className="trips-grid">

        {trips.length === 0 && (
          <p>{t("trips.noTrips")}</p>
        )}

        {trips.map(tItem => (

          <div
            key={tItem.id}
            className={`trip-card ${tItem.status === "Cancelled" ? "cancelled" : ""}`}
            onClick={() => setSelectedTrip(tItem)}
          >

            <h3>{tItem.tripType}</h3>

            {tItem.status === "Cancelled" && (
              <div className="trip-overlay">{t("trips.cancelled")}</div>
            )}

            {tItem.status === "Completed" && (
              <div className="trip-overlay completed">{t("trips.completed")}</div>
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

            {/* HEADER */}
            <div className="trip-modal-header">

              <h2>{selectedTrip.tripType} {t("trips.trip")}</h2>

              <button
                className="close-btn"
                onClick={() => setSelectedTrip(null)}
              >
                ✕
              </button>

            </div>

            {/* BODY */}
            <div className="trip-modal-body">

              <p>
                📅 {new Date(selectedTrip.startDate).toLocaleDateString()}
                {" → "}
                {new Date(selectedTrip.endDate).toLocaleDateString()}
              </p>

              <p>💰 {t("trips.budget")}: ${selectedTrip.budgetPerDay}</p>
              <p>👥 {t("trips.travelers")}: {selectedTrip.travelers}</p>
              <p>🚗 {t("trips.transport")}: {selectedTrip.transport}</p>

              {/* ITINERARY */}
              {selectedTrip.itineraryJson && (

                <div className="trip-itinerary">

                  <h3>🗺️ {t("trips.itinerary")}</h3>

                  <table className="itinerary-table">
                    <tbody>

                      {JSON.parse(selectedTrip.itineraryJson).map((day, dayIndex) => (
                        <React.Fragment key={dayIndex}>

                          <tr className="day-row">
                            <td colSpan="2">
                              {t("trips.day")} {day.day} — {day.region}
                            </td>
                          </tr>

                          {day.activities?.map(place => (
                            <tr key={place.id}>
                              <td className="time-col">{t("trips.activity")}</td>
                              <td>{place.name} — {place.location}</td>
                            </tr>
                          ))}

                          {day.restaurant && (
                            <tr>
                              <td className="time-col">{t("trips.restaurant")}</td>
                              <td>{day.restaurant.name}</td>
                            </tr>
                          )}

                        </React.Fragment>
                      ))}

                    </tbody>
                  </table>

                </div>

              )}

              <p>
                {t("trips.created")}{" "}
                {new Date(selectedTrip.createdAt).toLocaleDateString()}
              </p>

              <p className="trip-status">
                {t("trips.status")}: {selectedTrip.status || t("trips.active")}
              </p>

            </div>

            {/* FOOTER */}
            <div className="trip-modal-footer">

              {selectedTrip.status !== "Cancelled" && (
                <button
                  className="cancel-trip-btn"
                  onClick={() => cancelTrip(selectedTrip.id)}
                >
                  {t("trips.cancel")}
                </button>
              )}

            </div>

          </div>

        </div>

      )}

    </section>
  );
}