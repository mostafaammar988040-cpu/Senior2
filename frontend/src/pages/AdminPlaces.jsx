import { useState, useEffect } from "react";
import api from "../services/api";
import "../styles/AdminPlaces.css";

function AdminPlaces() {

  const [categories, setCategories] = useState([]);
  const [activityTypes, setActivityTypes] = useState([]);

  const [form, setForm] = useState({
    name: "",
    description: "",
    location: "",
    price: "",
    categoryId: "",
    activityTypeId: ""
  });

  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState(null);

  // ✅ Load categories
  useEffect(() => {
    api.get("/categories").then(res => setCategories(res.data));
  }, []);

  // ✅ Handle inputs
  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  // ✅ Handle category change
  const handleCategoryChange = async (e) => {
    const categoryId = e.target.value;

    setForm({
      ...form,
      categoryId,
      activityTypeId: ""
    });

    const selected = categories.find(c => c.id == categoryId);

    // 🔥 ONLY if Activities
    if (selected?.slug === "activities") {
      try {
        const res = await api.get(
          `/activitytypes/by-category?categoryId=${categoryId}`
        );
        setActivityTypes(res.data);
      } catch (err) {
        console.error("Error loading activity types:", err);
      }
    } else {
      setActivityTypes([]);
    }
  };

  // ✅ Image upload
  const handleImageChange = (e) => {
    const file = e.target.files[0];
    setImageFile(file);

    if (file) {
      setPreview(URL.createObjectURL(file));
    }
  };

  // ✅ Submit
  const handleSubmit = async (e) => {
    e.preventDefault();

    const data = new FormData();

    data.append("name", form.name);
    data.append("description", form.description);
    data.append("location", form.location);
    data.append("price", form.price || "");
    data.append("categoryId", form.categoryId);
    data.append("activityTypeId", form.activityTypeId || "");

    if (imageFile) {
      data.append("image", imageFile);
    }

    try {
      await api.post("/places", data, {
        headers: { "Content-Type": "multipart/form-data" }
      });

      alert("✅ Place added successfully!");

      // 🔄 reset
      setForm({
        name: "",
        description: "",
        location: "",
        price: "",
        categoryId: "",
        activityTypeId: ""
      });

      setImageFile(null);
      setPreview(null);
      setActivityTypes([]);

    } catch (err) {
      console.error(err);
      alert("❌ Error adding place");
    }
  };

  return (
    <div className="admin-places-page">

      <h1>✨ Add New Place</h1>

      <div className="admin-places-container">

        {/* FORM */}
        <form onSubmit={handleSubmit} className="admin-form">

          <input
            name="name"
            placeholder="Place Name"
            value={form.name}
            onChange={handleChange}
            required
          />

          <textarea
            name="description"
            placeholder="Description"
            value={form.description}
            onChange={handleChange}
          />

          <input
            name="location"
            placeholder="Location"
            value={form.location}
            onChange={handleChange}
          />

          <input
            name="price"
            placeholder="Price (optional)"
            value={form.price}
            onChange={handleChange}
          />

          {/* CATEGORY */}
          <select
            name="categoryId"
            value={form.categoryId}
            onChange={handleCategoryChange}
            required
          >
            <option value="">Select Category</option>
            {categories.map(c => (
              <option key={c.id} value={c.id}>{c.name}</option>
            ))}
          </select>

          {/* 🔥 ACTIVITY TYPE */}
          {activityTypes.length > 0 && (
            <>
              <label className="sub-label">Activity Type</label>

              <select
                name="activityTypeId"
                value={form.activityTypeId}
                onChange={handleChange}
              >
                <option value="">Select Activity Type</option>
                {activityTypes.map(a => (
                  <option key={a.id} value={a.id}>{a.name}</option>
                ))}
              </select>
            </>
          )}

          {/* IMAGE */}
          <label className="file-upload">
            📷 Upload Image
            <input type="file" accept="image/*" onChange={handleImageChange} />
          </label>

          <button type="submit">🚀 Add Place</button>

        </form>

        {/* PREVIEW */}
        <div className="preview-section">

          <div className="preview-card">

            <img
              src={preview || "https://via.placeholder.com/500x300"}
              alt="preview"
            />

            <div className="preview-content">
              <h2>{form.name || "Place Name"}</h2>
              <p>{form.description || "Description..."}</p>

              <div className="meta">
                <span>📍 {form.location || "Location"}</span>
                <span>💲 {form.price || "0"}</span>
              </div>

              {form.activityTypeId && (
                <span className="tag">🏷 Activity Selected</span>
              )}
            </div>

          </div>

        </div>

      </div>

    </div>
  );
}

export default AdminPlaces;