import { useState } from "react";
import api from "../services/api";
import "../styles/CreateJourney.css";

export default function CreateJourney() {
  const [formData, setFormData] = useState({
    title: "",
    content: "",
    media: null,
    isShared: false
  });

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const data = new FormData();

      data.append("Title", formData.title);
      data.append("Content", formData.content);
      data.append("IsShared", formData.isShared);

      if (formData.media) {
        data.append("Media", formData.media);
      }

      await api.post("/journey", data);

      alert("Journey created!");

      setFormData({
        title: "",
        content: "",
        media: null,
        isShared: false
      });

    } catch (err) {
      console.error(err);
      alert("Failed to create journey");
    }
  };

  return (
    <div className="create-journey-card">
      <h2>✈️ Write Your Journey</h2>

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Journey Title"
          value={formData.title}
          onChange={(e) =>
            setFormData({ ...formData, title: e.target.value })
          }
        />

        <textarea
          placeholder="Share your experience..."
          value={formData.content}
          onChange={(e) =>
            setFormData({ ...formData, content: e.target.value })
          }
        />

        <input
          type="file"
          accept="image/*,video/*"
          onChange={(e) =>
            setFormData({ ...formData, media: e.target.files[0] })
          }
        />

        <label className="checkbox">
          <input
            type="checkbox"
            checked={formData.isShared}
            onChange={(e) =>
              setFormData({ ...formData, isShared: e.target.checked })
            }
          />
          Share publicly
        </label>

        <button type="submit" className="btn-primary">
          Save Journey
        </button>
      </form>
    </div>
  );
}