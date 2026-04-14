import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/AdminPlacesList.css";

function AdminPlacesList() {

  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [places, setPlaces] = useState([]);

  // 🔹 Load categories
  useEffect(() => {
    api.get("/categories").then(res => setCategories(res.data));
  }, []);

  // 🔹 Load places when category clicked
  const handleCategoryClick = async (category) => {
    setSelectedCategory(category);

    const res = await api.get(`/places?category=${category.slug}`);
    setPlaces(res.data);
  };

  // 🔥 DELETE
  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("Delete this place?");
    if (!confirmDelete) return;

    await api.delete(`/places/${id}`);
    setPlaces(prev => prev.filter(p => p.id !== id));
  };

  return (
    <div className="admin-places-list">

      <h1>📍 Manage Places</h1>

      {/* 🔥 CATEGORY VIEW */}
      {!selectedCategory && (
        <div className="categories-grid">
          {categories.map(cat => (
            <div
              key={cat.id}
              className="category-card"
              onClick={() => handleCategoryClick(cat)}
            >
              <img src={cat.imageUrl} alt={cat.name} />
              <h2>{cat.name}</h2>
            </div>
          ))}
        </div>
      )}

      {/* 🔥 PLACES VIEW */}
      {selectedCategory && (
        <>
          <button
            className="back-btn"
            onClick={() => setSelectedCategory(null)}
          >
            ⬅ Back to Categories
          </button>

          <h2>{selectedCategory.name}</h2>

      <div className="places-grid">
  {places.map(place => (
    <div
      key={place.id}
      className="experience-card"
      style={{
        backgroundImage: `url(http://localhost:5262${place.imageUrl})`
      }}
    >
      <div className="overlay"></div>

      <div className="card-content">
        <h2>{place.name}</h2>
        <p>{place.location}</p>

        <button
          className="delete-btn"
          onClick={() => handleDelete(place.id)}
        >
          🗑 Delete
        </button>
      </div>
    </div>
  ))}
</div>
        </>
      )}

    </div>
  );
}

export default AdminPlacesList;