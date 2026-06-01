import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import api from "../services/api";
import "../styles/Recommendations.css";

export default function Recommendations() {
  const { t } = useTranslation();

  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get("/recommendations")
      .then(res => setRows(res.data))
      .catch(err => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  const openInGoogleMaps = (place) => {
    const location = place.location || place.city || "Lebanon";

    let query = "";

    if (place.lat && place.lng) {
      query = `${place.lat},${place.lng}`;
    } else {
      query = `${place.name} ${location}`;
    }

const googleUrl = `https://www.google.com/search?q=${encodeURIComponent(query)}`;
    window.open(googleUrl, "_blank", "noopener,noreferrer");
  };

  if (loading) {
    return (
      <div className="rec-page">
        <h1 className="rec-title">{t("rec.title")}</h1>
        <p className="loading">{t("rec.loading")}</p>
      </div>
    );
  }

  return (
    <div className="rec-page">

      <h1 className="rec-title">{t("rec.title")}</h1>

      {rows.length === 0 && (
        <p className="no-data">{t("rec.noRecommendations")}</p>
      )}

      {rows.map((row, index) => (
        <div key={index} className="rec-section">

          <h2 className="rec-section-title">{row.title}</h2>

          <div className="rec-row">

            {row.places?.length > 0 ? (
              row.places.map((place, i) => (
                <div
                  key={i}
                  className="rec-card"
                  onClick={() => openInGoogleMaps(place)}
                >
                  <img
                    src={place.imageUrl || "/images/default-place.jpg"}
                    alt={place.name || "Recommended place"}
                    loading="lazy"
                    onError={(e) => {
                      e.target.src = "/images/default-place.jpg";
                    }}
                  />

                  <div className="rec-info">
                    <h3>{place.name}</h3>
                    <p>{place.location || place.city || "Lebanon"}</p>

                    {place.rating && (
                      <span className="rec-rating">⭐ {place.rating}</span>
                    )}
                  </div>
                </div>
              ))
            ) : (
              <p className="no-data">{t("rec.noPlaces")}</p>
            )}

          </div>

        </div>
      ))}

    </div>
  );
}