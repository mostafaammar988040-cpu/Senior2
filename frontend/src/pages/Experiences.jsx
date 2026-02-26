import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Experiences.css";

export default function Experiences() {
  const [experiences, setExperiences] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    api.get("/categories")
      .then(res => setExperiences(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="experiences-page">

      {/* HERO */}
      <div className="experiences-hero">
        <h1>Curated Experiences</h1>
        <p>
          Discover unforgettable moments across Lebanon — from mountain
          adventures to coastal escapes.
        </p>
      </div>

      {/* GRID */}
      <div className="experiences-grid">
        {experiences.map(exp => (
          <div
            key={exp.id}
            className="experience-card"
onClick={() => {
  if (exp.slug === "activities") {
    navigate("/activities");
  } else {
    navigate(`/places?category=${exp.slug}`);
  }
}}            style={{
              backgroundImage: `url(${import.meta.env.VITE_API_BASE_URL}${exp.imageUrl})`
            }}
          >
            <div className="overlay"></div>
            <h2>{exp.name}</h2>
          </div>
        ))}
      </div>

    </div>
  );
}
