import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import ChromaGrid from "../components/ChromaGrid";
import "../styles/Events.css";

const Events = () => {
  const navigate = useNavigate();
  const [selectedType, setSelectedType] = useState("All");

  const eventTypes = ["All", "Sport", "History", "Tech", "Music"];

  return (
    <div className="events-page">
      {/* Header */}
      <header className="events-header">
        <h1 className="events-title">Upcoming Events in Lebanon</h1>
        <button className="back-button" onClick={() => navigate("/")}>
          Back to Home
        </button>
      </header>

      {/* Event type filter */}
      <div className="events-options">
        {eventTypes.map((type) => (
          <button
            key={type}
            className={`option-button ${selectedType === type ? "active" : ""}`}
            onClick={() => setSelectedType(type)}
          >
            {type}
          </button>
        ))}
      </div>

      {/* Event grid */}
      <main className="events-main">
        <ChromaGrid filterType={selectedType} />
      </main>
    </div>
  );
};

export default Events;