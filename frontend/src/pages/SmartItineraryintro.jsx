import React from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import "../styles/SmartItineraryintro.css";

const SmartItineraryintro = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  return (
    <div className="smart-hero">

      {/* Overlay */}
      <div className="overlay"></div>

      {/* Content */}
      <div className="hero-content">
        <h1>{t("smartIntro.title")}</h1>

        <p>{t("smartIntro.subtitle")}</p>

        <button
          className="start-btn"
          onClick={() => navigate("/SmartItinerary")}
        >
          {t("smartIntro.start")}
        </button>
      </div>

    </div>
  );
};

export default SmartItineraryintro;