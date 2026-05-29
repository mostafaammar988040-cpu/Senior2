import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Activities.css";
import { useTranslation } from "react-i18next";

export default function Activities() {

  const { t } = useTranslation();

  const [types, setTypes] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    api.get("/activitytypes")
      .then(res => setTypes(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="activities-page">

      <div className="activities-hero">
        <h1>{t("activities.title")}</h1>
        <p>{t("activities.subtitle")}</p>
      </div>

      <div className="activities-grid">
        {types.map(type => (
          <div
            key={type.id}
            className="activity-card"
            onClick={() =>
              navigate(`/places?activityType=${type.id}`)
            }
            style={{
              backgroundImage: `url(${import.meta.env.VITE_API_BASE_URL}${type.imageUrl})`
            }}
          >
            <div className="overlay"></div>
            <h2>{type.name}</h2> {/* dynamic from DB */}
          </div>
        ))}
      </div>

    </div>
  );
}