import { useEffect, useState } from "react";
import api from "../../services/api";
import "../../styles/ProfileFavorites.css";

export default function ProfileFavorites() {

  const [favorites, setFavorites] = useState([]);

  const user = JSON.parse(localStorage.getItem("user") || "null");

  useEffect(() => {
    if (!user) return;

    api.get(`/favorites/${user.id}`)
      .then(res => setFavorites(res.data))
      .catch(err => console.log(err));

  }, []);

  const removeFavorite = async (placeId) => {
    try {

      await api.delete(`/favorites/remove?userId=${user.id}&placeId=${placeId}`);

      setFavorites(prev => prev.filter(p => p.id !== placeId));

    } catch (err) {
      console.log(err);
    }
  };

  if (favorites.length === 0) {
    return (
      <div className="profile-fav-empty">
        <h3>No favorites yet 💔</h3>
        <p>Start adding places you love!</p>
      </div>
    );
  }

  return (
    <div className="profile-fav-container">

      <h2>Your Favorites ❤️</h2>

      <div className="profile-fav-grid">

        {favorites.map(place => (

          <div key={place.id} className="profile-fav-card">

            <img
              src={`${import.meta.env.VITE_API_BASE_URL}${place.imageUrl}`}
              alt={place.name}
            />

            <div className="profile-fav-content">
              <h3>{place.name}</h3>
              <p>{place.location}</p>

              <button
                className="remove-btn"
                onClick={() => removeFavorite(place.id)}
              >
                Remove ❌
              </button>
            </div>

          </div>

        ))}

      </div>

    </div>
  );
}