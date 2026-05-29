import { useEffect, useState } from "react";
import api from "../services/api";
import FollowButton from "./FollowButton";
import "../styles/Feed.css";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

export default function Feed() {
  const { t } = useTranslation();

  const [stories, setStories] = useState([]);
  const [journeys, setJourneys] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchFeed = async () => {
      try {
        const res = await api.get("/feed");

        const storiesData = res.data.filter(e => e.type === "story");
        const journeysData = res.data.filter(e => e.type === "journey");

        setStories(storiesData);
        setJourneys(journeysData);

      } catch (err) {
        console.error("Feed error:", err);
      }
    };

    fetchFeed();
  }, []);

  return (
    <div className="feed-page">

      {/* STORIES */}
      <div className="stories-section">
        <h3>{t("feed.stories")}</h3>

        <div className="stories-row">
          {stories.map((story) => (
            <div key={story.id} className="story-bubble">
              {story.mediaType === "video" ? (
                <video src={story.mediaUrl} />
              ) : (
                <img src={story.mediaUrl} alt="" />
              )}
            </div>
          ))}
        </div>
      </div>

      {/* HEADER */}
      <div className="feed-header">
        <h2>🌍 {t("feed.title")}</h2>

        <button 
          className="add-btn"
          onClick={() => navigate("/create-journey-entry")}
        >
          ➕ {t("feed.upload")}
        </button>
      </div>

      {/* JOURNEYS */}
      <div className="feed-section">
        {journeys.map((entry) => (
          <div key={entry.id} className="post-card">

            <div className="post-header">
              <span>{t("feed.user")} {entry.userId}</span>
            </div>

            <h4>{entry.title}</h4>
            <p>{entry.content}</p>

            {entry.mediaType === "image" && (
              <img src={entry.mediaUrl} alt="" />
            )}

            {entry.mediaType === "video" && (
              <video src={entry.mediaUrl} controls />
            )}

            <div className="post-actions">
              <FollowButton followedId={entry.userId} />
              <span>{new Date(entry.createdAt).toLocaleString()}</span>
            </div>

          </div>
        ))}
      </div>
    </div>
  );
}