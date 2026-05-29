import { useState } from "react";
import api from "../services/api";
import "../styles/FollowButton.css";

export default function FollowButton({ followedId }) {
  const [isFollowing, setIsFollowing] = useState(false);

  const handleFollow = async () => {
    try {
      if (isFollowing) {
        await api.delete(`/follow/${followedId}`);
        setIsFollowing(false);
      } else {
        await api.post(`/follow/${followedId}`);
        setIsFollowing(true);
      }
    } catch (err) {
      console.error(err);
      alert("Action failed");
    }
  };

  return (
    <button className="follow-btn" onClick={handleFollow}>
      {isFollowing ? "✓ Following" : "."}
    </button>
  );
}