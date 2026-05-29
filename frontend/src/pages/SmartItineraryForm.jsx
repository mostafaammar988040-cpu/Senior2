import { useState } from "react";
import api from "../services/api";
import "../styles/SmartItinerary.css";
import { useTranslation } from "react-i18next";

export default function SmartItineraryForm() {
  const { t } = useTranslation();

  const [budget, setBudget] = useState(110);
  const [travelers, setTravelers] = useState(1);

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [tripType, setTripType] = useState("Relaxing");
  const [transport, setTransport] = useState("Car");
  const [activities, setActivities] = useState([]);

  const [specialRequirements, setSpecialRequirements] = useState("");
  const [loading, setLoading] = useState(false);

  const [generatedDays, setGeneratedDays] = useState([]);
  const [includeSavedPlaces, setIncludeSavedPlaces] = useState(false);

  const toggleActivity = (activity) => {
    if (activities.includes(activity)) {
      setActivities(activities.filter((a) => a !== activity));
    } else {
      setActivities([...activities, activity]);
    }
  };

  const handleGenerate = async () => {
    if (!tripType) {
      alert(t("itinerary.selectType"));
      return;
    }

    if (!startDate || !endDate) {
      alert(t("itinerary.selectDates"));
      return;
    }

    const user = JSON.parse(localStorage.getItem("user"));

    if (!user || !user.id) {
      alert(t("itinerary.notLogged"));
      return;
    }

    try {
      setLoading(true);

      const payload = {
        userId: user.id,
        travelers: Number(travelers),
        startDate,
        endDate,
        budgetPerDay: Number(budget),
        tripType,
        activitiesJson: JSON.stringify(activities),
        transport,
        specialRequirements,
        includeSavedPlaces
      };

      const res = await api.post("/smartitinerary", payload);

      setGeneratedDays(res.data.itinerary || []);

      alert(t("itinerary.success"));
    } catch (err) {
      console.error(err);
      alert(t("itinerary.error"));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-container">

      <h2>{t("itinerary.basic")}</h2>

      <label>{t("itinerary.travelers")}</label>
      <select value={travelers} onChange={(e) => setTravelers(Number(e.target.value))}>
        <option value={1}>{t("itinerary.solo")}</option>
        <option value={2}>{t("itinerary.couple")}</option>
        <option value={4}>{t("itinerary.family")}</option>
        <option value={3}>{t("itinerary.friends")}</option>
      </select>

      <label>{t("itinerary.duration")}</label>
      <div className="date-row">
        <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
        <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
      </div>

      <h2>{t("itinerary.budget")}</h2>
      <div className="budget-card">
        <p>${budget} / {t("itinerary.day")}</p>
        <input type="range" min="20" max="500" value={budget} onChange={(e) => setBudget(Number(e.target.value))} />
      </div>

      <h2>{t("itinerary.tripType")}</h2>
      <div className="options-row">
        {["Relaxing", "Adventure", "Cultural", "Nightlife"].map((type) => (
          <button
            key={type}
            className={tripType === type ? "selected" : ""}
            onClick={() => setTripType(type)}
            type="button"
          >
            {t(`itinerary.types.${type}`)}
          </button>
        ))}
      </div>

      <h2>{t("itinerary.activities")}</h2>
      <div className="options-row">
        {["Beaches", "Mountains", "Historical Sites", "Hiking", "Food Experience", "Museums"].map((act) => (
          <button
            key={act}
            className={activities.includes(act) ? "selected" : ""}
            onClick={() => toggleActivity(act)}
            type="button"
          >
            {t(`itinerary.activitiesList.${act}`)}
          </button>
        ))}
      </div>

      <h2>{t("itinerary.transport")}</h2>
      <div className="options-row">
        {["Car", "Taxi / Uber", "Public Transport", "Walking Only"].map((tItem) => (
          <button
            key={tItem}
            className={transport === tItem ? "selected" : ""}
            onClick={() => setTransport(tItem)}
            type="button"
          >
            {t(`itinerary.transportList.${tItem}`)}
          </button>
        ))}
      </div>

      <h2>{t("itinerary.special")}</h2>
      <textarea
        placeholder={t("itinerary.specialPlaceholder")}
        value={specialRequirements}
        onChange={(e) => setSpecialRequirements(e.target.value)}
      />

      <div className="estimated-box">
        <h3>{t("itinerary.estimate")}</h3>
        <h1>${budget * travelers}</h1>
      </div>

      <h2 className="personal-title">{t("itinerary.personal")}</h2>
      <label className="saved-option">
        <input
          type="checkbox"
          checked={includeSavedPlaces}
          onChange={() => setIncludeSavedPlaces(!includeSavedPlaces)}
        />
        {t("itinerary.saved")}
      </label>

      <button className="generate-btn" onClick={handleGenerate} disabled={loading}>
        {loading ? t("itinerary.generating") : t("itinerary.generate")}
      </button>

      {generatedDays.length > 0 && (
        <>
          <h2>{t("itinerary.result")}</h2>

          {generatedDays.map((day) => (
            <div key={day.day} className="day-block">
              <h3>
                {t("itinerary.day")} {day.day} — {day.region}
              </h3>

              <div className="cards-grid">
                {day.activities?.map((place) => (
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
                  🍽 {t("itinerary.restaurant")}: {day.restaurant.name}
                </p>
              )}
            </div>
          ))}
        </>
      )}
    </div>
  );
}