import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/Journey.css";

export default function Journey() {
  const user = JSON.parse(localStorage.getItem("user") || "null");

  const [entries, setEntries] = useState([]);
  const [selected, setSelected] = useState(null);

  const [showCreate, setShowCreate] = useState(false);

  // CREATE STATES
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [mediaFile, setMediaFile] = useState(null);
  const [previewUrl, setPreviewUrl] = useState(null);

  // EDIT STATES
  const [isEditing, setIsEditing] = useState(false);
  const [editTitle, setEditTitle] = useState("");
  const [editContent, setEditContent] = useState("");
  const [editMediaFile, setEditMediaFile] = useState(null);
  const [editPreviewUrl, setEditPreviewUrl] = useState(null);

  // LOAD JOURNEYS
  useEffect(() => {
    if (!user?.id) return;

    let cancelled = false;

    (async () => {
      const res = await api.get(`/journey/${user.id}`);
      if (!cancelled) setEntries(res.data);
    })();

    return () => { cancelled = true; };
  }, [user?.id]);

  const loadJourneys = async () => {
    if (!user?.id) return;
    const res = await api.get(`/journey/${user.id}`);
    setEntries(res.data);
  };

  const openView = (entry) => {
    setSelected(entry);
    setIsEditing(false);
    setEditTitle(entry.title || "");
    setEditContent(entry.content || "");
    setEditPreviewUrl(entry.mediaUrl || null);
  };

  const closeView = () => {
    setSelected(null);
    setIsEditing(false);
  };

  // ========================
  // CREATE JOURNEY
  // ========================
  const handleCreate = async () => {
    if (!user?.id) return alert("You must be logged in");
    if (!title.trim() || !content.trim())
      return alert("Please enter title + content");

    const formData = new FormData();
    formData.append("userId", user.id);
    formData.append("title", title);
    formData.append("content", content);

    if (mediaFile) {
      formData.append("media", mediaFile);
    }

    await api.post("/journey", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });

    setTitle("");
    setContent("");
    setMediaFile(null);
    setPreviewUrl(null);
    setShowCreate(false);

    loadJourneys();
  };

  // ========================
  // EDIT JOURNEY
  // ========================
  const handleSaveEdit = async () => {
    if (!user?.id || !selected?.id) return;

    const formData = new FormData();
    formData.append("userId", user.id);
    formData.append("title", editTitle);
    formData.append("content", editContent);

    if (editMediaFile) {
      formData.append("media", editMediaFile);
    }

    const res = await api.put(`/journey/${selected.id}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });

    setSelected(res.data);
    setIsEditing(false);

    setEntries((prev) =>
      prev.map((e) => (e.id === res.data.id ? res.data : e))
    );
  };

  const detectMediaType = (url) =>
    url?.toLowerCase().includes(".mp4") ? "video" : "image";

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

        <button className="btnPrimary" onClick={() => setShowCreate(true)}>
          + New Journey
        </button>
      </div>

      {/* GRID */}
      <div className="titleGrid">
        {entries.length === 0 ? (
          <div className="emptyState">
            <div className="emptyEmoji">✨</div>
            <h3>No journeys yet</h3>
            <p>Create your first memory and keep it forever.</p>
            <button
              className="btnPrimary"
              onClick={() => setShowCreate(true)}
            >
              Create Journey
            </button>
          </div>
        ) : (
          entries.map((e) => (
            <div key={e.id} className="titleCard" onClick={() => openView(e)}>
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
          ))
        )}
      </div>

      {/* CREATE MODAL */}
      {showCreate && (
        <div className="modalOverlay" onClick={() => setShowCreate(false)}>
          <div
            className="modalShell"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="modalHeader">
              <h2>Create Journey</h2>
              <button
                className="btnGhost"
                onClick={() => setShowCreate(false)}
              >
                ✕
              </button>
            </div>

            <div className="formGrid">
              <div className="field">
                <label>Title</label>
                <input
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                />
              </div>

              <div className="field">
                <label>Story</label>
                <textarea
                  value={content}
                  onChange={(e) => setContent(e.target.value)}
                />
              </div>

              <div className="field">
                <label>Upload Media (optional)</label>
                <input
                  type="file"
                  accept="image/*,video/*"
                  onChange={(e) => {
                    const file = e.target.files[0];
                    if (file) {
                      setMediaFile(file);
                      setPreviewUrl(URL.createObjectURL(file));
                    }
                  }}
                />
              </div>

              {previewUrl && (
                <div className="mediaWrap">
                  {mediaFile?.type.startsWith("video") ? (
                    <video src={previewUrl} controls />
                  ) : (
                    <img src={previewUrl} alt="preview" />
                  )}
                </div>
              )}
            </div>

            <div className="modalActions">
              <button
                className="btnSoft"
                onClick={() => setShowCreate(false)}
              >
                Cancel
              </button>
              <button className="btnPrimary" onClick={handleCreate}>
                Save
              </button>
            </div>
          </div>
        </div>
      )}

      {/* VIEW / EDIT MODAL */}
      {selected && (
        <div className="modalOverlay" onClick={closeView}>
          <div
            className="modalShell modalWide"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="modalHeader">
              <div>
                <h2>
                  {isEditing ? "Edit Journey" : selected.title}
                </h2>
                <p className="muted">
                  {new Date(selected.createdAt).toLocaleString()}
                </p>
              </div>
              <button className="btnGhost" onClick={closeView}>
                ✕
              </button>
            </div>

            {!isEditing && selected.mediaUrl && (
              <div className="mediaWrap">
                {selected.mediaType === "video" ? (
                  <video src={selected.mediaUrl} controls />
                ) : (
                  <img src={selected.mediaUrl} alt={selected.title} />
                )}
              </div>
            )}

            {!isEditing ? (
              <div className="contentBlock">
                <p className="contentText">{selected.content}</p>
              </div>
            ) : (
              <div className="formGrid">
                <div className="field">
                  <label>Title</label>
                  <input
                    value={editTitle}
                    onChange={(e) => setEditTitle(e.target.value)}
                  />
                </div>

                <div className="field">
                  <label>Story</label>
                  <textarea
                    value={editContent}
                    onChange={(e) =>
                      setEditContent(e.target.value)
                    }
                  />
                </div>

                <div className="field">
                  <label>Replace Media (optional)</label>
                  <input
                    type="file"
                    accept="image/*,video/*"
                    onChange={(e) => {
                      const file = e.target.files[0];
                      if (file) {
                        setEditMediaFile(file);
                        setEditPreviewUrl(
                          URL.createObjectURL(file)
                        );
                      }
                    }}
                  />
                </div>

                {editPreviewUrl && (
                  <div className="mediaWrap">
                    {detectMediaType(editPreviewUrl) === "video" ? (
                      <video src={editPreviewUrl} controls />
                    ) : (
                      <img src={editPreviewUrl} alt="preview" />
                    )}
                  </div>
                )}
              </div>
            )}

            <div className="modalActions">
              {!isEditing ? (
                <>
                  <button className="btnSoft" onClick={closeView}>
                    Close
                  </button>
                  <button
                    className="btnPrimary"
                    onClick={() => setIsEditing(true)}
                  >
                    Edit
                  </button>
                </>
              ) : (
                <>
                  <button
                    className="btnSoft"
                    onClick={() => setIsEditing(false)}
                  >
                    Cancel
                  </button>
                  <button
                    className="btnPrimary"
                    onClick={handleSaveEdit}
                  >
                    Save Changes
                  </button>
                </>
              )}
            </div>
          </div>
        </div>
      )}
    </section>
  );
}