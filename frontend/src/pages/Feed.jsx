import { useEffect, useState } from "react";
import api from "../services/api";
import FollowButton from "./FollowButton";
import "../styles/Feed.css";

export default function Feed() {
  const [feed, setFeed] = useState([]);

  useEffect(() => {
    const fetchFeed = async () => {
      try {
        const res = await api.get("/feed");
        setFeed(res.data);
      } catch (err) {
        console.error("Failed to fetch feed:", err);
      }
    };
    fetchFeed();
  }, []);

  return (
    <div className="feed-container">
      <h2>🌍 Inspiration Feed</h2>

      {feed.length === 0 ? (
        <p>No shared journeys yet.</p>
      ) : (
        <div className="feed-grid">
          {feed.map((entry) => (
            <div key={entry.id} className="feed-card">
              <h3>{entry.title}</h3>
              <p>{entry.content}</p>

              {entry.mediaUrl && entry.mediaType === "image" && (
                <img src={entry.mediaUrl} alt={entry.title} />
              )}

              {entry.mediaUrl && entry.mediaType === "video" && (
                <video controls src={entry.mediaUrl}></video>
              )}

              <FollowButton followedId={entry.userId} />

              <span className="timestamp">
                {new Date(entry.createdAt).toLocaleString()}
              </span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}