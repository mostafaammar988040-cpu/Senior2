import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import api from "../services/api";
import "../styles/Preferences.css";

function Preferences() {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [step, setStep] = useState(1);

  const user = JSON.parse(localStorage.getItem("user") || "null");

  const [selected, setSelected] = useState({
    interests: [],
    food: [],
    activities: []
  });

  // ✅ SAFE IMAGE MAP (fixes ALL missing images)
  const imageMap = {
    interests: {
      cultural: "/images/preferences/interests/cultural.jpg",
      historical: "/images/preferences/interests/historical.jpg",
      nightlife: "/images/preferences/interests/nightLife.jpg"
    },
    food: {
      lebanese: "/images/preferences/food/lebanese.jpg",
      seafood: "/images/preferences/food/seafood.jpg",
      vegetarian: "/images/preferences/food/vegeterian.jpg" // keep your file name
    },
    activities: {
      beaches: "/images/preferences/activities/beaches.jpg",
      hiking: "/images/preferences/activities/hike.jpg", // fix mismatch
      skiing: "/images/preferences/activities/skiing.jpg"
    }
  };

  const dataMap = {
    interests: ["cultural", "historical", "nightlife"],
    food: ["lebanese", "seafood", "vegetarian"],
    activities: ["beaches", "hiking", "skiing"]
  };

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

  const category =
    step === 1 ? "interests" :
    step === 2 ? "food" :
    "activities";

  const currentData = dataMap[category];

  const title =
    step === 1 ? t("pref.title1") :
    step === 2 ? t("pref.title2") :
    t("pref.title3");

  return (
    <div className="pref-container">
      <div className="pref-panel">

        {/* Progress */}
        <div className="progress-bar">
          <div
            className="progress-fill"
            style={{ width: `${(step / 3) * 100}%` }}
          ></div>
        </div>

        <h2>{title}</h2>

        <div className="card-grid">

          {currentData.map(id => (
            <div
              key={id}
              className={`pref-card ${
                selected[category].includes(id) ? "selected" : ""
              }`}
              onClick={() => toggleSelect(category, id)}
            >

              {/* ✅ FIXED IMAGE SOURCE */}
              <img
                src={imageMap[category][id]}
                alt={id}
                onError={(e) => {
                  e.target.src = "/images/default-place.jpg";
                }}
              />

              <span>{t(`pref.${category}.${id}`)}</span>

            </div>
          ))}

        </div>

        <div className="pref-actions">

          <button
            className="skip-btn"
            onClick={() => navigate("/introduction")}
          >
            {t("pref.skip")}
          </button>

          {step < 3 && (
            <button
              className="continue-btn"
              onClick={nextStep}
            >
              {t("pref.continue")}
            </button>
          )}

          {step === 3 && (
            <button
              className="continue-btn"
              onClick={finish}
            >
              {t("pref.finish")}
            </button>
          )}

        </div>

      </div>
    </div>
  );
}

export default Preferences;