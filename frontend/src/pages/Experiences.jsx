import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Experiences.css";
import { useTranslation } from "react-i18next"; // ✅ ADD

export default function Experiences() {

  const { t } = useTranslation(); // ✅ ADD

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
        <h1>{t("experiences.title")}</h1> {/* ✅ FIX */}
        <p>{t("experiences.subtitle")}</p> {/* ✅ FIX */}
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
            }}
            style={{
              backgroundImage: `url(${import.meta.env.VITE_API_BASE_URL}${exp.imageUrl})`
            }}
          >
            <div className="overlay"></div>
            <h2>{exp.name}</h2> {/* dynamic → keep */}
          </div>
        ))}
      </div>

    </div>
  );
}