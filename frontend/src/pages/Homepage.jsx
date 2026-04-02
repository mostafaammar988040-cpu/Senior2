import { useTranslation } from "react-i18next";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Homepage.css";

import heroImg from "../assets/landscape.webp";
import baalbekImg from "../assets/baalbek.jpg";
import byblosImg from "../assets/byblos.jpg";
import cedarsImg from "../assets/cedars.jpg";

export default function Home() {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [showPopup, setShowPopup] = useState(false);

  // 🔥 Show popup only if NOT logged in
  useEffect(() => {
    const token = localStorage.getItem("token");
    const seen = sessionStorage.getItem("seenPopup");

    if (!token && !seen) {
      setShowPopup(true);
      sessionStorage.setItem("seenPopup", "true");
    }
  }, []);

  return (
    <>
      {/* HERO */}
      <section className="hero">
        <img src={heroImg} className="hero-bg" alt="Hero Background" />

        <div className="hero-text">
          <h1>{t("home.heroTitle")}</h1>
          <p>{t("home.heroText")}</p>
          <button>{t("home.startJourney")}</button>
        </div>
      </section>

      {/* STRIP */}
      <div className="strip">
        <div>✨ {t("home.features.itinerary")}</div>
        <div>🤖 {t("home.features.assistant")}</div>
        <div>🗺️ {t("home.features.map")}</div>
        <div>🎉 {t("home.features.events")}</div>
      </div>

      {/* DESTINATIONS */}
      <section className="explore-section">
        <h2>{t("home.topDestinations")}</h2>

        <div className="explore-grid">
          <div className="explore-card">
            <img src={baalbekImg} alt="Baalbek" />
            <h3>{t("home.baalbek")}</h3>
          </div>

          <div className="explore-card">
            <img src={byblosImg} alt="Byblos" />
            <h3>{t("home.byblos")}</h3>
          </div>

          <div className="explore-card">
            <img src={cedarsImg} alt="Cedars" />
            <h3>{t("home.cedars")}</h3>
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="home-actions">
        <h2>{t("home.ctaTitle")}</h2>

        <div className="home-buttons">
          <button onClick={() => window.location.href = "/explore"}>
            {t("home.explore")}
          </button>

          <button onClick={() => window.location.href = "/help"}>
            {t("home.help")}
          </button>
        </div>
      </section>

      {/* FOOTER */}
      <footer>
        {t("home.footer")}
      </footer>

      {/* 🔥 POPUP */}
      {showPopup && (
        <div className="popup-overlay">
          <div className="popup-box">
            <h2>Welcome 👋</h2>
            <p>You can access the rest of features after logging in 🔐</p>

            <div className="popup-buttons">
              <button onClick={() => navigate("/login")}>
                Login
              </button>

              <button onClick={() => setShowPopup(false)}>
                Continue as Guest
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}