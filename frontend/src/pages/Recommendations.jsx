import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Recommendations.css";

export default function Recommendations() {

  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {

    api.get("/recommendations")
      .then(res => {
        setRows(res.data);
      })
      .catch(err => console.error(err))
      .finally(() => setLoading(false));

  }, []);

  if (loading) {
    return (
      <div className="rec-page">
        <h1 className="rec-title">Recommended For You</h1>
        <p className="loading">Loading recommendations...</p>
      </div>
    );
  }

  return (
    <div className="rec-page">

      <h1 className="rec-title">Recommended For You</h1>

      {rows.map((row, index) => (

        <div key={index} className="rec-section">

          <h2 className="rec-section-title">{row.title}</h2>

          <div className="rec-row">

            {row.places?.length > 0 && row.places.map((place, i) => (

              <div
                key={i}
                className="rec-card"
                onClick={() => navigate(`/places/${place.id}`)}
              >

                <img
                  src={place.imageUrl}
                  alt={place.name}
                  loading="lazy"
                  onError={(e) => {
                    e.target.src = "/images/default-place.jpg";
                  }}
                />

                <div className="rec-info">
                  <h3>{place.name}</h3>
                  <p>{place.city}</p>
                </div>

              </div>

            ))}

          </div>

        </div>

      ))}

    </div>
  );
}