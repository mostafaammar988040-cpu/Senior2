import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import api from "../services/api";
import "../styles/Explore.css";

export default function Explore() {
  const [places, setPlaces] = useState([]);
  const [searchParams] = useSearchParams();
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
    <div className="explore-page">

      <div className="explore-hero">
        <h1>{category ? category.toUpperCase() : "Explore Lebanon"}</h1>
        <p>
          Discover handpicked destinations across Lebanon.
        </p>
      </div>

      <div className="explore-grid">
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
