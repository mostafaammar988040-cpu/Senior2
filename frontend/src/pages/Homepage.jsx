import { useTranslation } from "react-i18next";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Homepage.css";

import heroImg from "../assets/landscape.webp";
import baalbekImg from "../assets/baalbek.jpg";
import byblosImg from "../assets/byblos.jpg";
import cedarsImg from "../assets/cedars.jpg";

export default function Home() {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [showPopup, setShowPopup] = useState(false);
  const [sponsoredAds, setSponsoredAds] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const seen = sessionStorage.getItem("seenPopup");

    if (!token && !seen) {
      setShowPopup(true);
      sessionStorage.setItem("seenPopup", "true");
    }
  }, []);

  useEffect(() => {
    api
      .get("/Advertisement/active")
      .then((res) => {
        setSponsoredAds(res.data);
      })
      .catch((err) => {
        console.error("Failed to load sponsored ads:", err);
      });
  }, []);

  const getImageUrl = (imageUrl) => {
    if (!imageUrl) return byblosImg;

    if (imageUrl.startsWith("http")) {
      return imageUrl;
    }

    return `${import.meta.env.VITE_API_BASE_URL || "https://localhost:7090"}${imageUrl}`;
  };

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

      {/* SPONSORED ADS FROM BACKEND */}
      {sponsoredAds.length > 0 && (
        <section className="ads-section">
          <div className="ads-header">
            <span className="ads-label">Sponsored</span>
            <h2>Featured Places in Lebanon</h2>
            <p>Handpicked recommendations to inspire your next visit.</p>
          </div>

          <div className="ads-row">
            {sponsoredAds.map((ad) => {
              const place = ad.place || {};

              const title =
                place.name ||
                ad.placeName ||
                ad.title ||
                "Sponsored Place";

              const description =
                place.description ||
                ad.description ||
                "Discover this featured destination in Lebanon.";

              const image =
                place.imageUrl ||
                ad.imageUrl ||
                ad.placeImageUrl ||
                null;

              return (
                <div className="ad-card" key={ad.id}>
                  <div className="ad-image-wrap">
                    <img src={getImageUrl(image)} alt={title} />
                    <span className="ad-badge">Ad</span>
                  </div>

                  <div className="ad-content">
                    <h3>{title}</h3>
                    <p>{description}</p>

                    <button
                      onClick={() => {
                        if (place.id || ad.placeId) {
                          navigate(`/places/${place.id || ad.placeId}`);
                        }
                      }}
                    >
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        </section>
      )}

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
          <button onClick={() => navigate("/explore")}>
            {t("home.explore")}
          </button>

          <button onClick={() => navigate("/help")}>
            {t("home.help")}
          </button>
        </div>
      </section>

      {/* FOOTER */}
      <footer>{t("home.footer")}</footer>

      {/* LOGIN POPUP */}
      {showPopup && (
        <div className="popup-overlay">
          <div className="popup-box">
            <h2>{t("popup.title")}</h2>
            <p>{t("popup.text")}</p>

            <div className="popup-buttons">
              <button onClick={() => navigate("/login")}>
                {t("popup.login")}
              </button>

              <button onClick={() => setShowPopup(false)}>
                {t("popup.guest")}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}