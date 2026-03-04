import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Journey.css";

export default function Journey() {

  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem("user") || "null");

  const [entries, setEntries] = useState([]);
  const [loading, setLoading] = useState(true);

  const [selectedJourney, setSelectedJourney] = useState(null);
  const [isEditing, setIsEditing] = useState(false);

  const [editTitle, setEditTitle] = useState("");
  const [editContent, setEditContent] = useState("");

  const [editMediaFile, setEditMediaFile] = useState(null);
  const [editPreviewUrl, setEditPreviewUrl] = useState(null);

  // ================= UPDATE JOURNEY =================
  const updateJourney = async () => {

    if (!selectedJourney) return;

    try {

      const formData = new FormData();

      formData.append("title", editTitle);
      formData.append("content", editContent);
      formData.append("userId", user.id);

      if (editMediaFile) {
        formData.append("media", editMediaFile);
      }

      const res = await api.put(`/journey/${selectedJourney.id}`, formData, {
        headers: { "Content-Type": "multipart/form-data" }
      });

      const updated = res.data;

      setEntries(prev =>
        prev.map(j => j.id === updated.id ? updated : j)
      );

      setSelectedJourney(updated);
      setIsEditing(false);
      setEditMediaFile(null);
      setEditPreviewUrl(null);

    } catch (err) {
      console.error(err);
      alert("Failed to update journey");
    }

  };

  // ================= LOAD JOURNEYS =================
  useEffect(() => {

    if (!user?.id) return;

    let cancelled = false;

    (async () => {

      try {

        const res = await api.get(`/journey/${user.id}`);

        if (!cancelled) {
          setEntries(res.data || []);
        }

      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }

    })();

    return () => {
      cancelled = true;
    };

  }, [user?.id]);

  if (loading) {
    return (
      <section className="journeyPage">
        <p>Loading journeys...</p>
      </section>
    );
  }

  return (
    <section className="journeyPage">

      {/* HEADER */}
      <div className="journeyTop">

        <div className="journeyTitleBlock">

          <div className="journeyIcon">🧭</div>

          <div>
            <h1>My Travel Journal</h1>
            <p>Capture moments, memories, and adventures in Lebanon.</p>
          </div>

        </div>

        <button
          className="btnPrimary"
          onClick={() => navigate("/profile/journey/new")}
        >
          + New Journey
        </button>

      </div>

      {/* BANNER */}
      <div className="journeyBanner">

        <div className="bannerText">
          <h2>Write your Lebanon story</h2>
          <p>Every place you visit becomes part of your travel journal.</p>
        </div>

      </div>

      {/* GRID */}
      <div className="titleGrid">

        {entries.length === 0 && (

          <div className="emptyState">

            <div className="emptyEmoji">✨</div>

            <h3>No journeys yet</h3>

            <p>Create your first memory and keep it forever.</p>

            <button
              className="btnPrimary"
              onClick={() => navigate("/profile/journey/new")}
            >
              Create Journey
            </button>

          </div>

        )}

        {entries.map((e) => (

          <div
            key={e.id}
            className="titleCard"
            onClick={() => {
              setSelectedJourney(e);
              setEditTitle(e.title);
              setEditContent(e.content);
              setEditPreviewUrl(null);
              setEditMediaFile(null);
              setIsEditing(false);
            }}
          >

            {e.mediaUrl && (
              <div className="cardMedia">

                {e.mediaType === "video" ? (
                  <video src={e.mediaUrl} muted />
                ) : (
                  <img src={e.mediaUrl} alt={e.title} />
                )}

              </div>
            )}

            <div className="cardContent">

              <div className="titleCardTop">

                <span className="pill">
                  {new Date(e.createdAt).toLocaleDateString()}
                </span>

                <span className="arrow">↗</span>

              </div>

              <h3 className="titleText">{e.title}</h3>

              <p className="snippet">
                {(e.content || "").slice(0, 90)}
                {(e.content || "").length > 90 ? "..." : ""}
              </p>

            </div>

          </div>

        ))}

      </div>

      {/* JOURNAL MODAL */}

      {selectedJourney && (

        <div
          className="journalModalOverlay"
          onClick={() => setSelectedJourney(null)}
        >

          <div
            className="journalModalPaper"
            onClick={(e) => e.stopPropagation()}
          >

            <div className="editorToolbar">

              <button
                className="btnBack"
                onClick={() => {
                  setSelectedJourney(null);
                  setIsEditing(false);
                }}
              >
                ← Close
              </button>

              {!isEditing ? (

                <button
                  className="btnSave"
                  onClick={() => setIsEditing(true)}
                >
                  Edit
                </button>

              ) : (

                <button
                  className="btnSave"
                  onClick={updateJourney}
                >
                  Save Changes
                </button>

              )}

            </div>

            {/* TITLE */}

            {!isEditing ? (

              <h1 className="journalTitle">
                {selectedJourney.title}
              </h1>

            ) : (

              <input
                className="journalTitle"
                value={editTitle}
                onChange={(e) => setEditTitle(e.target.value)}
              />

            )}

            {/* MEDIA */}

            {!isEditing && selectedJourney.mediaUrl && (

              <div className="mediaWrap">

                {selectedJourney.mediaType === "video" ? (
                  <video src={selectedJourney.mediaUrl} controls />
                ) : (
                  <img src={selectedJourney.mediaUrl} alt={selectedJourney.title} />
                )}

              </div>

            )}

            {/* CONTENT */}

            {!isEditing ? (

              <div className="journalContentView">
                {selectedJourney.content}
              </div>

            ) : (

              <>
                <textarea
                  className="journalContent"
                  value={editContent}
                  onChange={(e) => setEditContent(e.target.value)}
                />

                <div className="field">

                  <label>Replace Media (optional)</label>

                  <input
                    type="file"
                    accept="image/*,video/*"
                    onChange={(e) => {

                      const file = e.target.files[0];

                      if (!file) return;

                      setEditMediaFile(file);
                      setEditPreviewUrl(URL.createObjectURL(file));

                    }}
                  />

                </div>

                {editPreviewUrl && (

                  <div className="mediaWrap">

                    {editMediaFile?.type.startsWith("video") ? (
                      <video src={editPreviewUrl} controls />
                    ) : (
                      <img src={editPreviewUrl} alt="preview" />
                    )}

                  </div>

                )}

              </>

            )}

          </div>

        </div>

      )}

    </section>
  );
}