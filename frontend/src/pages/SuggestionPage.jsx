import { useState } from "react";
import api from "../services/api";
import "../styles/SuggestionPage.css";
import { useTranslation } from "react-i18next";

export default function SuggestionPage() {
  const { t } = useTranslation();

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
      alert(t("suggestion.loginRequired"));
      return;
    }

    if (!type || !title || !description) {
      alert(t("suggestion.fillForm"));
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
        headers: { "Content-Type": "multipart/form-data" }
      });

      setSubmitted(true);

      setTitle("");
      setDescription("");
      setLocation("");
      setImage(null);
      setPreview(null);

    } catch (err) {
      console.error(err);
      alert(t("suggestion.error"));
    }
  };

  return (
    <section className="suggestionPage">

      {/* HERO */}
      <div className="suggestionHero">
        <h1>{t("suggestion.title")}</h1>
        <p>{t("suggestion.subtitle")}</p>
      </div>

      {/* TYPES */}
      {!type && (
        <div className="suggestionTypes">

          <div className="typeCard" onClick={() => setType("place")}>
            <span>📍</span>
            <h3>{t("suggestion.types.place.title")}</h3>
            <p>{t("suggestion.types.place.desc")}</p>
          </div>

          <div className="typeCard" onClick={() => setType("feature")}>
            <span>⚙️</span>
            <h3>{t("suggestion.types.feature.title")}</h3>
            <p>{t("suggestion.types.feature.desc")}</p>
          </div>

          <div className="typeCard" onClick={() => setType("bug")}>
            <span>🐞</span>
            <h3>{t("suggestion.types.bug.title")}</h3>
            <p>{t("suggestion.types.bug.desc")}</p>
          </div>

          <div className="typeCard" onClick={() => setType("general")}>
            <span>💡</span>
            <h3>{t("suggestion.types.general.title")}</h3>
            <p>{t("suggestion.types.general.desc")}</p>
          </div>

        </div>
      )}

      {/* FORM */}
      {type && !submitted && (
        <div className="suggestionForm">

          <h2>{t("suggestion.formTitle")}</h2>

          <input
            placeholder={t("suggestion.titleInput")}
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />

          <textarea
            placeholder={t("suggestion.descriptionInput")}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />

          <input
            placeholder={t("suggestion.location")}
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
            <img className="previewImage" src={preview} alt="preview" />
          )}

          <button className="submitSuggestion" onClick={submitSuggestion}>
            {t("suggestion.submit")}
          </button>

        </div>
      )}

      {/* SUCCESS */}
      {submitted && (
        <div className="suggestionSuccess">

          <h2>{t("suggestion.successTitle")}</h2>

          <p>{t("suggestion.successText")}</p>

          <button
            className="submitSuggestion"
            onClick={() => {
              setSubmitted(false);
              setType(null);
            }}
          >
            {t("suggestion.another")}
          </button>

        </div>
      )}

    </section>
  );
}