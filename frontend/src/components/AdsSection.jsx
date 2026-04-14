import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./AdsSection.css";

const API_BASE = "https://localhost:7090/api";
const PLACEHOLDER =
  "https://images.unsplash.com/photo-1469474968028-56623f02e42e?auto=format&fit=crop&w=1200&q=80";

const AdsSection = () => {
  const [ads, setAds] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const controller = new AbortController();

    const fetchAds = async () => {
      try {
        setLoading(true);
        setError("");

        const { data } = await axios.get(`${API_BASE}/Advertisement/active`, {
          signal: controller.signal,
        });

        const normalized = Array.isArray(data)
          ? data
              .filter((item) => item && item.isActive)
              .sort((a, b) => (b.priority ?? 0) - (a.priority ?? 0))
          : data && Array.isArray(data.$values)
          ? data.$values
              .filter((item) => item && item.isActive)
              .sort((a, b) => (b.priority ?? 0) - (a.priority ?? 0))
          : [];

        setAds(normalized);
      } catch (err) {
        if (err.name !== "CanceledError" && err.name !== "AbortError") {
          console.log("Ads fetch error:", err);
          setError("Could not load sponsored places right now.");
        }
      } finally {
        setLoading(false);
      }
    };

    fetchAds();
    return () => controller.abort();
  }, []);

  return (
    <section className="ads-section" aria-labelledby="sponsored-places-title">
      <div className="ads-section__header">
        <h2 id="sponsored-places-title">🔥 Sponsored Places</h2>
        <p>Hand-picked premium experiences just for you</p>
      </div>

      {loading && (
        <div className="ads-row ads-row--loading">
          {Array.from({ length: 4 }).map((_, i) => (
            <div key={i} className="ad-card ad-card--skeleton" />
          ))}
        </div>
      )}

      {!loading && error && <p className="ads-section__error">{error}</p>}

      {!loading && !error && ads.length === 0 && (
        <div className="ads-row">
          <article className="ad-card">
            <span className="ad-card__badge">🔥 Sponsored</span>
            <img
              className="ad-card__image"
              src={PLACEHOLDER}
              alt="Sponsored placeholder"
            />
            <div className="ad-card__content">
              <h3>No sponsored places yet</h3>
            </div>
          </article>
        </div>
      )}

      {!loading && !error && ads.length > 0 && (
        <div className="ads-row" role="list" aria-label="Sponsored places list">
          {ads.map((ad) => (
            <article
              key={ad.id}
              className="ad-card"
              role="listitem"
              onClick={() => navigate(`/places/${ad.placeId}`)}
            >
              <span className="ad-card__badge">🔥 Sponsored</span>

              <img
                className="ad-card__image"
                src={`https://picsum.photos/seed/place-${ad.placeId}/720/420`}
                onError={(e) => {
                  e.currentTarget.src = PLACEHOLDER;
                }}
                alt={ad.placeName ? `${ad.placeName} sponsored place` : "Sponsored place"}
                loading="lazy"
              />

              <div className="ad-card__content">
                <h3 title={ad.placeName}>{ad.placeName || "Sponsored Place"}</h3>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  );
};

export default AdsSection;