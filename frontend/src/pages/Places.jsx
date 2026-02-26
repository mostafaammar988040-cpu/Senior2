import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Places.css";

export default function Places() {
  const [places, setPlaces] = useState([]);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const category = searchParams.get("category");
  const activityType = searchParams.get("activityType");

  useEffect(() => {
    let url = "/places?";

    if (category) {
      url += `category=${category}&`;
    }

    if (activityType) {
      url += `activityType=${activityType}`;
    }

    api.get(url)
      .then(res => setPlaces(res.data))
      .catch(err => console.error(err));

  }, [category, activityType]);

  return (
    <div className="places-page">

      {/* BACK BUTTON */}
      <button className="back-btn" onClick={() => navigate(-1)}>
        ← Back
      </button>

      {/* HERO */}
      <div className="places-hero">
        <h1>
          {category ? category.toUpperCase() : "Explore Places"}
        </h1>
        <p>
          Discover amazing destinations across Lebanon.
        </p>
      </div>

      {/* GRID */}
      <div className="places-grid">
        {places.map(place => (
          <div key={place.id} className="place-card">

            <div className="place-image-wrapper">
              <img
                src={`${import.meta.env.VITE_API_BASE_URL}${place.imageUrl}`}
                alt={place.name}
              />
            </div>

            <div className="place-content">
              <h3>{place.name}</h3>
              <p>{place.location}</p>
              <span>${place.price}</span>
            </div>

          </div>
        ))}
      </div>

    </div>
  );
}