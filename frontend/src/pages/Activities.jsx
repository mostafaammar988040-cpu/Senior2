import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Activities.css";

export default function Activities() {

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
        <h1>Activities</h1>
        <p>Choose your adventure type</p>
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
            <h2>{type.name}</h2>
          </div>
        ))}
      </div>

    </div>
  );
}