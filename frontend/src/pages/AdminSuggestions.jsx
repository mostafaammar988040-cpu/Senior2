import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/AdminSuggestions.css";

export default function AdminSuggestions() {

  const [suggestions, setSuggestions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedSuggestion, setSelectedSuggestion] = useState(null);

  useEffect(() => {

    const fetchSuggestions = async () => {

      try {

        const res = await api.get("/suggestion");

        setSuggestions(res.data || []);
        setLoading(false);

      } catch (error) {

        console.error("Error fetching suggestions:", error);
        setLoading(false);

      }

    };

    fetchSuggestions();

  }, []);

  return (
    <div className="admin-suggestions">

      <h1>User Suggestions</h1>

      {loading && <p>Loading suggestions...</p>}

      <div className="suggestions-grid">

        {suggestions.map((s) => (

          <div
            key={s.id}
            className="suggestion-card"
            onClick={() => setSelectedSuggestion(s)}
          >

            <h3>{s.title}</h3>

            <div className="suggestion-meta">

              <span>👤 {s.userName}</span>

              <span>
                📅 {new Date(s.createdAt).toLocaleDateString()}
              </span>

            </div>

          </div>

        ))}

      </div>

      {/* MODAL */}

      {selectedSuggestion && (

        <div
          className="suggestion-modal-overlay"
          onClick={() => setSelectedSuggestion(null)}
        >

          <div
            className="suggestion-modal"
            onClick={(e) => e.stopPropagation()}
          >

            <h2>{selectedSuggestion.title}</h2>

            <p className="modal-user">
              👤 {selectedSuggestion.userName}
            </p>

            <p>
              <strong>Type:</strong> {selectedSuggestion.type}
            </p>

            <p>
              <strong>Description:</strong> {selectedSuggestion.description}
            </p>

            {selectedSuggestion.location && (
              <p>
                <strong>Location:</strong> {selectedSuggestion.location}
              </p>
            )}

            {selectedSuggestion.imageUrl && (
              <img
                src={`https://localhost:5001${selectedSuggestion.imageUrl}`}
                alt="suggestion"
                className="modal-image"
              />
            )}

            <button
              className="close-modal"
              onClick={() => setSelectedSuggestion(null)}
            >
              Close
            </button>

          </div>

        </div>

      )}

    </div>
  );
}