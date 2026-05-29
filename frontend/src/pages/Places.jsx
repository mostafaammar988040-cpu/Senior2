import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import api from "../services/api";
import "../styles/Places.css";

export default function Places() {

  const { t } = useTranslation();

  const [places, setPlaces] = useState([]);
  const [filtered, setFiltered] = useState([]);
  const [selectedPlace, setSelectedPlace] = useState(null);

  const [reviews, setReviews] = useState([]);
  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState("");

  const [search, setSearch] = useState("");
  const [cityFilter, setCityFilter] = useState("");

  const [weather, setWeather] = useState(null);
  const [loadingWeather, setLoadingWeather] = useState(false);

  const [isFavorite, setIsFavorite] = useState(false);

  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const category = searchParams.get("category");
  const activityType = searchParams.get("activityType");

  const user = JSON.parse(localStorage.getItem("user") || "null");

  useEffect(() => {
    if (!selectedPlace) return;

    setWeather(null);
    setLoadingWeather(true);

    const city = selectedPlace.location + " Lebanon";

    api.get(`/weather/${city}`)
      .then(res => setWeather(res.data))
      .catch(() => setWeather(null))
      .finally(() => setLoadingWeather(false));

  }, [selectedPlace]);

  // FETCH PLACES
  useEffect(() => {

    let url = "/places?";

    if (category) url += `category=${category}&`;
    if (activityType) url += `activityType=${activityType}`;

    api.get(url)
      .then(res => {
        setPlaces(res.data);
        setFiltered(res.data);
      });

  }, [category, activityType]);

  // SEARCH + FILTER
  useEffect(() => {

    let result = places;

    if (search) {
      result = result.filter(p =>
        p.name.toLowerCase().includes(search.toLowerCase())
      );
    }

    if (cityFilter) {
      result = result.filter(p =>
        p.location.toLowerCase().includes(cityFilter.toLowerCase())
      );
    }

    setFiltered(result);

  }, [search, cityFilter, places]);

  // REVIEWS
  const loadReviews = (placeId) => {
    api.get(`/reviews/${placeId}`)
      .then(res => setReviews(res.data));
  };

  const submitReview = async () => {
    if (!user) {
      alert(t("places.login"));
      return;
    }

    await api.post("/reviews", {
      placeId: selectedPlace.id,
      userId: user.id,
      rating,
      comment
    });

    setComment("");
    loadReviews(selectedPlace.id);
  };

  // FAVORITES
  useEffect(() => {
    if (!user || !selectedPlace) return;

    api.get(`/favorites/${user.id}`)
      .then(res => {
        const exists = res.data.some(p => p.id === selectedPlace.id);
        setIsFavorite(exists);
      });

  }, [selectedPlace]);

  const handleFavorite = async () => {
    if (!user) {
      alert(t("places.login"));
      return;
    }

    try {
      if (!isFavorite) {
        await api.post(`/favorites/add?userId=${user.id}&placeId=${selectedPlace.id}`);
        setIsFavorite(true);
      } else {
        await api.delete(`/favorites/remove?userId=${user.id}&placeId=${selectedPlace.id}`);
        setIsFavorite(false);
      }
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <div className="places-page">

      <button className="back-btn" onClick={() => navigate(-1)}>
        ← {t("places.back")}
      </button>

      <div className="places-hero">
        <h1>{category ? category.toUpperCase() : t("places.title")}</h1>
        <p>{t("places.subtitle")}</p>
      </div>

      {/* SEARCH */}
      <div className="places-controls">

        <input
          placeholder={t("places.search")}
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <input
          placeholder={t("places.filter")}
          value={cityFilter}
          onChange={(e) => setCityFilter(e.target.value)}
        />

      </div>

      {/* GRID */}
      <div className="places-grid">

        {filtered.map(place => (
          <div
            key={place.id}
            className="place-card"
            onClick={() => {
              setSelectedPlace(place);
              loadReviews(place.id);
            }}
          >

            <div className="place-image-wrapper">

              <img
                src={`${import.meta.env.VITE_API_BASE_URL}${place.imageUrl}`}
                alt={place.name}
              />

              <div className="place-hover-overlay">
                <button className="view-details-btn">
                  {t("places.view")}
                </button>
              </div>

            </div>

            <div className="place-content">
              <h3>{place.name}</h3>
              <p>{place.location}</p>
            </div>

          </div>
        ))}

      </div>

      {/* MODAL */}
      {selectedPlace && (

        <div className="place-modal-overlay" onClick={() => setSelectedPlace(null)}>

          <div className="place-modal-card" onClick={(e) => e.stopPropagation()}>

            <button className="close-btn" onClick={() => setSelectedPlace(null)}>✕</button>

            <img
              className="modal-image"
              src={`${import.meta.env.VITE_API_BASE_URL}${selectedPlace.imageUrl}`}
            />

            <h2>{selectedPlace.name}</h2>

            <p className="modal-description">
              {selectedPlace.description}
            </p>

            <div className="modal-meta">
              <p><strong>{t("places.location")}:</strong> {selectedPlace.location}</p>

              {loadingWeather && <p>{t("places.loadingWeather")}</p>}

              {weather && (
                <div className="weather-box">
                  <img src={weather.icon} alt="weather" />
                  <span>{weather.temp}°C — {weather.condition}</span>
                </div>
              )}
            </div>

            {/* ACTIONS */}
            <div className="modal-actions">

              <a
                href={`https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(selectedPlace.location)}`}
                target="_blank"
                rel="noopener noreferrer"
                className="maps-btn"
              >
                {t("places.maps")}
              </a>

              <a
                href={`https://www.google.com/search?q=${encodeURIComponent(selectedPlace.name + " booking Lebanon")}`}
                target="_blank"
                rel="noopener noreferrer"
                className="booking-btn"
              >
                {t("places.book")}
              </a>

              <a
                href={`https://www.google.com/search?q=${encodeURIComponent(selectedPlace.name + " contact phone Lebanon")}`}
                target="_blank"
                rel="noopener noreferrer"
                className="contact-btn"
              >
                {t("places.contact")}
              </a>

              <button
                className="add-trip-btn"
                onClick={async () => {

                  const user = JSON.parse(localStorage.getItem("user"));

                  await api.post(
                    `/trips/add-place?userId=${user.id}&placeId=${selectedPlace.id}`
                  );

                  alert(t("places.addedTrip"));

                }}
              >
                {t("places.addTrip")}
              </button>

              <button
                className="favorite-btn"
                onClick={handleFavorite}
              >
                {isFavorite
                  ? t("places.removeFav")
                  : t("places.addFav")}
              </button>

            </div>

            {/* REVIEWS */}
            <div className="reviews-section">

              <h3>{t("places.reviews")}</h3>

              {reviews.map(r => (
                <div key={r.id} className="review-card">

                  <div className="review-top">
                    <strong>{r.user}</strong>
                    <span>{"⭐".repeat(r.rating)}</span>
                  </div>

                  <p>{r.comment}</p>

                </div>
              ))}

              <div className="review-form">

                <select
                  value={rating}
                  onChange={(e) => setRating(Number(e.target.value))}
                >
                  <option value="5">⭐⭐⭐⭐⭐</option>
                  <option value="4">⭐⭐⭐⭐</option>
                  <option value="3">⭐⭐⭐</option>
                  <option value="2">⭐⭐</option>
                  <option value="1">⭐</option>
                </select>

                <textarea
                  placeholder={t("places.writeReview")}
                  value={comment}
                  onChange={(e) => setComment(e.target.value)}
                />

                <button onClick={submitReview}>
                  {t("places.submit")}
                </button>

              </div>

            </div>

          </div>

        </div>

      )}

    </div>
  );
}