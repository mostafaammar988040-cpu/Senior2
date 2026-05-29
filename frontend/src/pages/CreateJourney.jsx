import { useState } from "react";
import api from "../services/api";
import "../styles/CreateJourney.css";
import { useTranslation } from "react-i18next";

export default function CreateJourney() {
  const { t } = useTranslation();

  const [formData, setFormData] = useState({
    title: "",
    content: "",
    media: null,
    isShared: false
  });

  const [preview, setPreview] = useState(null);
  const [isStory, setIsStory] = useState(false);

  const handleFile = (file) => {
    setFormData({ ...formData, media: file });

    if (file) {
      const url = URL.createObjectURL(file);
      setPreview(url);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const data = new FormData();

      if (isStory) {
        data.append("media", formData.media);
        await api.post("/journey/story", data);
      } else {
        data.append("Title", formData.title);
        data.append("Content", formData.content);
        data.append("IsShared", formData.isShared);

        if (formData.media) {
          data.append("Media", formData.media);
        }

        await api.post("/journey", data);
      }

      alert(t("journey.success"));

      setFormData({
        title: "",
        content: "",
        media: null,
        isShared: false
      });
      setPreview(null);

    } catch (err) {
      console.error(err);
      alert(t("journey.error"));
    }
  };

  return (
    <div className="create-page">

      <div className="creator-card">

        <h1>{t("journey.title")}</h1>

        {/* Tabs */}
        <div className="tabs">
          <button
            className={!isStory ? "active" : ""}
            onClick={() => setIsStory(false)}
          >
            {t("journey.tabJourney")}
          </button>
          <button
            className={isStory ? "active" : ""}
            onClick={() => setIsStory(true)}
          >
            {t("journey.tabStory")}
          </button>
        </div>

        <form onSubmit={handleSubmit}>

          {!isStory && (
            <>
              <input
                className="input"
                type="text"
                placeholder={t("journey.titlePlaceholder")}
                value={formData.title}
                onChange={(e) =>
                  setFormData({ ...formData, title: e.target.value })
                }
              />

              <textarea
                className="textarea"
                placeholder={t("journey.contentPlaceholder")}
                value={formData.content}
                onChange={(e) =>
                  setFormData({ ...formData, content: e.target.value })
                }
              />
            </>
          )}

          {/* Upload */}
          <div className="upload-box">
            <label className="upload-box">
              <input
                type="file"
                accept="image/*,video/*"
                hidden
                onChange={(e) => handleFile(e.target.files[0])}
              />

              {!preview ? (
                <div className="upload-placeholder">
                  <span className="icon">📸</span>
                  <p>{t("journey.upload")}</p>
                  <small>{t("journey.format")}</small>
                </div>
              ) : (
                <div className="preview">
                  {formData.media?.type.startsWith("video") ? (
                    <video src={preview} controls />
                  ) : (
                    <img src={preview} alt="preview" />
                  )}
                </div>
              )}
            </label>
          </div>

          {/* Preview */}
          {preview && (
            <div className="preview">
              {formData.media?.type.startsWith("video") ? (
                <video src={preview} controls />
              ) : (
                <img src={preview} alt="preview" />
              )}
            </div>
          )}

          {!isStory && (
            <label className="share-toggle">
              <input
                type="checkbox"
                checked={formData.isShared}
                onChange={(e) =>
                  setFormData({ ...formData, isShared: e.target.checked })
                }
              />

              <div className="share-text">
                <strong>{t("journey.share")}</strong>
                <span>{t("journey.shareDesc")}</span>
              </div>
            </label>
          )}

          <button className="submit-btn">
            {isStory ? t("journey.postStory") : t("journey.publish")}
          </button>

        </form>
      </div>
    </div>
  );
}