import { useState } from "react";
import api from "../services/api";
import "../styles/SmartItinerary.css";

export default function SmartItineraryForm() {

  /* ===== STATES ===== */
  const [budget, setBudget] = useState(110);
  const [travelers, setTravelers] = useState("Solo");

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [tripType, setTripType] = useState(null);
  const [activities, setActivities] = useState([]);
  const [transport, setTransport] = useState(null);

  const [specialRequirements, setSpecialRequirements] = useState("");

  const [loading, setLoading] = useState(false);

  /* ===== TOGGLE ACTIVITIES ===== */
  const toggleActivity = (activity) => {
    if (activities.includes(activity)) {
      setActivities(activities.filter(a => a !== activity));
    } else {
      setActivities([...activities, activity]);
    }
  };

  /* ===== GENERATE TRIP ===== */
  const handleGenerate = async () => {
    try {
      setLoading(true);

      const payload = {
        userId: 1, // later replace with logged user
        travelers,
        startDate,
        endDate,
        budgetPerDay: budget,
        tripType,
        activities,
        transport,
        specialRequirements
      };

      const res = await api.post("/smartitinerary", payload);

      console.log("Generated trip:", res.data);

      alert("Trip generated successfully!");

    } catch (err) {
      console.error(err);
      alert("Failed to generate trip");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-container">

      {/* BASIC DETAILS */}
      <h2>📋 Basic Trip Details</h2>

      <label>Number of Travelers</label>
      <select
        value={travelers}
        onChange={(e) => setTravelers(e.target.value)}
      >
        <option>Solo</option>
        <option>Couple</option>
        <option>Family</option>
        <option>Friends</option>
      </select>

      <label>Trip Duration</label>
      <div className="date-row">
        <input
          type="date"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
        />
        <input
          type="date"
          value={endDate}
          onChange={(e) => setEndDate(e.target.value)}
        />
      </div>

      {/* BUDGET */}
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

      {/* TRIP TYPE */}
      <h2>✨ What Type of Trip Are You Looking For?</h2>

      <div className="options-row">
        {["Relaxing", "Adventure", "Cultural", "Nightlife"].map(type => (
          <button
            key={type}
            className={tripType === type ? "selected" : ""}
            onClick={() => setTripType(type)}
          >
            {type}
          </button>
        ))}
      </div>

      {/* ACTIVITIES */}
      <h2>🌍 What Activities Do You Prefer?</h2>

      <div className="options-row">
        {[
          "Beaches",
          "Mountains",
          "Historical Sites",
          "Hiking",
          "Food Experience",
          "Museums",
        ].map(act => (
          <button
            key={act}
            className={activities.includes(act) ? "selected" : ""}
            onClick={() => toggleActivity(act)}
          >
            {act}
          </button>
        ))}
      </div>

      {/* TRANSPORT */}
      <h2>🚗 Preferred Transportation</h2>

      <div className="options-row">
        {["Car", "Taxi / Uber", "Public Transport", "Walking Only"].map(t => (
          <button
            key={t}
            className={transport === t ? "selected" : ""}
            onClick={() => setTransport(t)}
          >
            {t}
          </button>
        ))}
      </div>

      {/* SPECIAL */}
      <h2>📝 Special Requirements</h2>

      <textarea
        placeholder="Any allergies, accessibility needs, kids, seniors, etc."
        value={specialRequirements}
        onChange={(e) => setSpecialRequirements(e.target.value)}
      />

      {/* ESTIMATED */}
      <div className="estimated-box">
        <h3>📄 Estimated Trip Cost</h3>
        <h1>${budget * 2}</h1>
      </div>

      {/* GENERATE */}
      <button
        className="generate-btn"
        onClick={handleGenerate}
        disabled={loading}
      >
        {loading ? "Generating..." : "Generate My Trip Plan"}
      </button>

    </div>
  );
}