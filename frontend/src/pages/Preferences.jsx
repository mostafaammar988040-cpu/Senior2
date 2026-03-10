import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Preferences.css";

const interests = [
  { id: "cultural", label: "Cultural", img: "/images/preferences/interests/cultural.jpg" },
  { id: "historical", label: "Historical", img: "/images/preferences/interests/historical.jpg" },
  { id: "nightlife", label: "Nightlife", img: "/images/preferences/interests/nightLife.jpg" }
];

const food = [
  { id: "lebanese", label: "Lebanese", img: "/images/preferences/food/lebanese.jpg" },
  { id: "seafood", label: "Seafood", img: "/images/preferences/food/seafood.jpg" },
  { id: "vegeterian", label: "Vegetarian", img: "/images/preferences/food/vegeterian.jpg" }
];

const activities = [
  { id: "beaches", label: "Beaches", img: "/images/preferences/activities/beaches.jpg" },
  { id: "hike", label: "Hiking", img: "/images/preferences/activities/hike.jpg" },
  { id: "skiing", label: "Skiing", img: "/images/preferences/activities/skiing.jpg" }
];

function Preferences() {

  const navigate = useNavigate();
  const [step, setStep] = useState(1);

  const user = JSON.parse(localStorage.getItem("user") || "null");

  const [selected, setSelected] = useState({
    interests: [],
    food: [],
    activities: []
  });

  const toggleSelect = (category, id) => {
    setSelected(prev => {
      const exists = prev[category].includes(id);

      return {
        ...prev,
        [category]: exists
          ? prev[category].filter(item => item !== id)
          : [...prev[category], id]
      };
    });
  };

  const nextStep = () => setStep(step + 1);

  const finish = async () => {

    try {

      await api.post("/preferences", {
        userId: user.id,
        preferences: selected
      });

      navigate("/introduction");

    } catch (err) {
      console.log(err);
    }

  };

  const currentData =
    step === 1 ? interests :
    step === 2 ? food :
    activities;

  const category =
    step === 1 ? "interests" :
    step === 2 ? "food" :
    "activities";

  const title =
    step === 1 ? "What places do you like?" :
    step === 2 ? "What food do you enjoy?" :
    "What activities excite you?";

  return (
    <div className="pref-container">

      <div className="pref-panel">

      <div className="progress-bar">
        <div
          className="progress-fill"
          style={{ width: `${(step / 3) * 100}%` }}
        ></div>
      </div>

      <h2>{title}</h2>

      <div className="card-grid">

        {currentData.map(item => (

          <div
            key={item.id}
            className={`pref-card ${
              selected[category].includes(item.id) ? "selected" : ""
            }`}
            onClick={() => toggleSelect(category, item.id)}
          >

            <img src={item.img} alt={item.label} />
            <span>{item.label}</span>

          </div>

        ))}

      </div>

      <div className="pref-actions">

        <button
          className="skip-btn"
          onClick={() => navigate("/introduction")}
        >
          Skip
        </button>

        {step < 3 && (
          <button
            className="continue-btn"
            onClick={nextStep}
          >
            Continue
          </button>
        )}

        {step === 3 && (
          <button
            className="continue-btn"
            onClick={finish}
          >
            Finish
          </button>
        )}

      </div>

      </div>

    </div>
  );
}

export default Preferences;