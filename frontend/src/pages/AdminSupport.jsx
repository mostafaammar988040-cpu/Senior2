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

        if (Array.isArray(res.data)) {
          setSuggestions(res.data);
        } else if (res.data && Array.isArray(res.data.$values)) {
          setSuggestions(res.data.$values);
        } else {
          setSuggestions([]);
        }
      } catch (error) {
        console.error("Error fetching suggestions:", error);
        setSuggestions([]);
      } finally {
        setLoading(false);
      }
    };

    fetchSuggestions();
  }, []);

  const getImageUrl = (imageUrl) => {
    if (!imageUrl) return "";

    if (imageUrl.startsWith("http")) {
      return imageUrl;
    }

    return `https://localhost:7090${imageUrl}`;
  };

  return (
    <div className="admin-suggestions">
      <h1>User Suggestions</h1>

      {loading && <p>Loading suggestions...</p>}

      {!loading && suggestions.length === 0 && (
        <p>No suggestions found.</p>
      )}

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

            <p className="suggestion-type">{s.type}</p>
          </div>
        ))}
      </div>

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

            {selectedSuggestion.userEmail && (
              <p>
                <strong>Email:</strong> {selectedSuggestion.userEmail}
              </p>
            )}

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
                src={getImageUrl(selectedSuggestion.imageUrl)}
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