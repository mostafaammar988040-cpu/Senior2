import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import ChromaGrid from "../components/ChromaGrid";
import "../styles/Events.css";
import { useTranslation } from "react-i18next";

const Events = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [selectedType, setSelectedType] = useState("All");

  const eventTypes = ["All", "Sport", "History", "Tech", "Music"];

  return (
    <div className="events-page">

      {/* Header */}
      <header className="events-header">
        <h1 className="events-title">{t("events.title")}</h1>

        <button
          className="back-button"
          onClick={() => navigate("/")}
        >
          {t("events.back")}
        </button>
      </header>

      {/* Filter */}
      <div className="events-options">
        {eventTypes.map((type) => (
          <button
            key={type}
            className={`option-button ${selectedType === type ? "active" : ""}`}
            onClick={() => setSelectedType(type)}
          >
            {t(`events.types.${type}`)}
          </button>
        ))}
      </div>

      {/* Grid */}
      <main className="events-main">
        <ChromaGrid filterType={selectedType} />
      </main>
    </div>
  );
};

export default Events;