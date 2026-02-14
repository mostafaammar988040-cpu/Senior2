import { useState } from "react";
import "../styles/SmartItinerary.css";

export default function SmartItineraryForm() {
  const [budget, setBudget] = useState(110);

  return (
    <div className="form-container">

      {/* Basic Trip Details */}
      <h2>📋 Basic Trip Details</h2>

      <label>Number of Travelers</label>
      <select>
        <option>Solo</option>
        <option>Couple</option>
        <option>Family</option>
        <option>Friends</option>
      </select>

      <label>Trip Duration</label>
      <div className="date-row">
        <input type="date" />
        <input type="date" />
      </div>

      {/* Budget */}
      <h2>💰 Your Budget Per Day</h2>

      <div className="budget-card">
        <p>${budget} / day</p>
        <input
          type="range"
          min="20"
          max="500"
          value={budget}
          onChange={(e) => setBudget(e.target.value)}
        />
      </div>

      {/* Trip Type */}
      <h2>✨ What Type of Trip Are You Looking For?</h2>

      <div className="options-row">
        <button>Relaxing</button>
        <button>Adventure</button>
        <button>Cultural</button>
        <button>Nightlife</button>
      </div>

      {/* Activities */}
      <h2>🌍 What Activities Do You Prefer?</h2>

      <div className="options-row">
        <button>Beaches</button>
        <button>Mountains</button>
        <button>Historical Sites</button>
        <button>Hiking</button>
        <button>Food Experience</button>
        <button>Museums</button>
      </div>

      {/* Transport */}
      <h2>🚗 Preferred Transportation</h2>

      <div className="options-row">
        <button>Car</button>
        <button>Taxi / Uber</button>
        <button>Public Transport</button>
        <button>Walking Only</button>
      </div>

      {/* Special Requirements */}
      <h2>📝 Special Requirements</h2>
      <textarea placeholder="Any allergies, accessibility needs, kids, seniors, etc." />

      {/* Estimated Cost */}
      <div className="estimated-box">
        <h3>📄 Estimated Trip Cost</h3>
        <h1>${budget * 2}</h1>
      </div>

      <button className="generate-btn">
        Generate My Trip Plan
      </button>

    </div>
  );
}
