import { useState } from "react";
import api from "../services/api";
import "../styles/SmartItinerary.css";

export default function SmartItineraryForm() {

  const [budget, setBudget] = useState(110);
  const [travelers, setTravelers] = useState("Solo");

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [tripType, setTripType] = useState(null);
  const [activities, setActivities] = useState([]);
  const [transport, setTransport] = useState(null);

  const [specialRequirements, setSpecialRequirements] = useState("");
  const [loading, setLoading] = useState(false);

  const [generatedDays, setGeneratedDays] = useState([]);

  const [includeSavedPlaces, setIncludeSavedPlaces] = useState(false);

  const toggleActivity = (activity) => {
    if (activities.includes(activity)) {
      setActivities(activities.filter(a => a !== activity));
    } else {
      setActivities([...activities, activity]);
    }
  };

  const handleGenerate = async () => {

    if (!tripType) {
      alert("Please select a trip type");
      return;
    }

    if (!startDate || !endDate) {
      alert("Please select trip dates");
      return;
    }

    try {

      setLoading(true);

      const user = JSON.parse(localStorage.getItem("user"));

      const payload = {
        userId: user?.id || 1,
        travelers,
        startDate,
        endDate,
        budgetPerDay: budget,
        tripType,
        activitiesJson: JSON.stringify(activities),
        transport,
        specialRequirements,
        includeSavedPlaces
      };

      const res = await api.post("/smartitinerary", payload);

      setGeneratedDays(res.data.itinerary || []);

      alert("Trip generated successfully!");

    } catch (err) {
      console.error(err);
      alert("Failed to generate trip");
    }
    finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-container">

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

      <h2>✨ What Type of Trip Are You Looking For?</h2>

      <div className="options-row">
        {["Relaxing", "Adventure", "Cultural", "Nightlife"].map(type => (
          <button
            key={type}
            className={tripType === type ? "selected" : ""}
            onClick={() => setTripType(type)}
            type="button"
          >
            {type}
          </button>
        ))}
      </div>

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
            type="button"
          >
            {act}
          </button>
        ))}
      </div>

      <h2>🚗 Preferred Transportation</h2>

      <div className="options-row">
        {["Car", "Taxi / Uber", "Public Transport", "Walking Only"].map(t => (
          <button
            key={t}
            className={transport === t ? "selected" : ""}
            onClick={() => setTransport(t)}
            type="button"
          >
            {t}
          </button>
        ))}
      </div>

      <h2>📝 Special Requirements</h2>

      <textarea
        placeholder="Any allergies, accessibility needs, kids, seniors, etc."
        value={specialRequirements}
        onChange={(e) => setSpecialRequirements(e.target.value)}
      />

      <div className="estimated-box">
        <h3>📄 Estimated Trip Cost</h3>
        <h1>${budget * 2}</h1>
      </div>

      <h2 className="personal-title">⭐ Personal Options</h2>

      <label className="saved-option">
        <input
          type="checkbox"
          checked={includeSavedPlaces}
          onChange={() => setIncludeSavedPlaces(!includeSavedPlaces)}
        />
        Include places I saved while exploring
      </label>

      <button
        className="generate-btn"
        onClick={handleGenerate}
        disabled={loading}
      >
        {loading ? "Generating..." : "Generate My Trip Plan"}
      </button>

      {generatedDays.length > 0 && (
        <>
          <h2>🗺️ Your AI Trip Plan</h2>

          {generatedDays.map(day => (

  <div key={day.day} className="day-block">

    <h3>Day {day.day} — {day.region}</h3>

    <div className="cards-grid">

      {day.activities?.map(place => (

        <div key={place.id} className="card">

          <img src={place.imageUrl} alt={place.name} />

          <h4>{place.name}</h4>

          <p>{place.location}</p>

          <p>{place.activityType}</p>

        </div>

      ))}

    </div>

    {day.restaurant && (
      <p className="day-restaurant">
        🍽 Restaurant: {day.restaurant.name}
      </p>
    )}

  </div>

))}
        </>
      )}

    </div>
  );
}