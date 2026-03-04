import { useState } from "react";
import api from "../services/api";
import "../styles/SuggestionPage.css";

export default function SuggestionPage() {

  const user = JSON.parse(localStorage.getItem("user") || "{}");

  const [type, setType] = useState(null);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [location, setLocation] = useState("");

  const [image, setImage] = useState(null);
  const [preview, setPreview] = useState(null);

  const [submitted, setSubmitted] = useState(false);

  const submitSuggestion = async () => {

    if (!user?.id) {
      alert("You must be logged in");
      return;
    }

    if (!type || !title || !description) {
      alert("Please complete the form");
      return;
    }

    const formData = new FormData();

    formData.append("userId", user.id);
    formData.append("type", type);
    formData.append("title", title);
    formData.append("description", description);
    formData.append("location", location);

    if (image) {
      formData.append("image", image);
    }

    try {

      await api.post("/suggestion", formData, {
        headers: {
          "Content-Type": "multipart/form-data"
        }
      });

      setSubmitted(true);

      setTitle("");
      setDescription("");
      setLocation("");
      setImage(null);
      setPreview(null);

    } catch (err) {

      console.error(err);
      alert("Failed to send suggestion");

    }

  };

  return (

    <section className="suggestionPage">

      {/* HERO */}

      <div className="suggestionHero">

        <h1>💡 Help Improve Lebanon Travel</h1>

        <p>
          Your ideas help us discover new places,
          improve features, and build the best tourism
          platform for Lebanon.
        </p>

      </div>

      {/* CATEGORY SELECTION */}

      {!type && (

        <div className="suggestionTypes">

          <div
            className="typeCard"
            onClick={() => setType("place")}
          >
            <span>📍</span>
            <h3>Suggest a Place</h3>
            <p>Restaurant, hike, beach, hidden gem</p>
          </div>

          <div
            className="typeCard"
            onClick={() => setType("feature")}
          >
            <span>⚙️</span>
            <h3>Suggest Feature</h3>
            <p>New idea for the platform</p>
          </div>

          <div
            className="typeCard"
            onClick={() => setType("bug")}
          >
            <span>🐞</span>
            <h3>Report Bug</h3>
            <p>Something isn't working</p>
          </div>

          <div
            className="typeCard"
            onClick={() => setType("general")}
          >
            <span>💡</span>
            <h3>General Idea</h3>
            <p>Any suggestion to improve</p>
          </div>

        </div>

      )}

      {/* FORM */}

      {type && !submitted && (

        <div className="suggestionForm">

          <h2>Share Your Idea</h2>

          <input
            placeholder="Suggestion title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />

          <textarea
            placeholder="Describe your suggestion..."
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />

          <input
            placeholder="Location (optional)"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
          />

          <input
            type="file"
            accept="image/*"
            onChange={(e) => {

              const file = e.target.files[0];

              if (!file) return;

              setImage(file);
              setPreview(URL.createObjectURL(file));

            }}
          />

          {preview && (

            <img
              className="previewImage"
              src={preview}
              alt="preview"
            />

          )}

          <button
            className="submitSuggestion"
            onClick={submitSuggestion}
          >
            Submit Suggestion
          </button>

        </div>

      )}

      {/* SUCCESS MESSAGE */}

      {submitted && (

        <div className="suggestionSuccess">

          <h2>🎉 Thank you!</h2>

          <p>
            Your suggestion helps improve the travel
            experience in Lebanon.
          </p>

          <button
            className="submitSuggestion"
            onClick={() => {
              setSubmitted(false);
              setType(null);
            }}
          >
            Send Another Suggestion
          </button>

        </div>

      )}

    </section>

  );

}