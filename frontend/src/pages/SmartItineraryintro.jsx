import React from "react";
import { useNavigate } from "react-router-dom";
import "../styles/SmartItineraryintro.css";

const SmartItineraryintro = () => {
  const navigate = useNavigate();

  return (
    <div className="smart-hero">

      {/* Overlay */}
      <div className="overlay"></div>

      {/* Content */}
      <div className="hero-content">
        <h1>Plan Your Lebanese Adventure</h1>
        <p>
          Let us create a smart, personalized itinerary based on your choices.
        </p>

        <button
          className="start-btn"
         onClick={() => navigate("/SmartItinerary")}

        >
          Start Planning
        </button>
      </div>

    </div>
  );
};

export default SmartItineraryintro;
