import { useEffect, useState } from "react";
import api from "../services/api";
import "../styles/FollowButton.css";

export default function FollowButton({ followedId, initialIsFollowing = false }) {
  const [isFollowing, setIsFollowing] = useState(initialIsFollowing);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setIsFollowing(initialIsFollowing);
  }, [initialIsFollowing]);

  const handleFollow = async () => {
    if (loading) return;

    try {
      setLoading(true);

      if (isFollowing) {
        const res = await api.delete(`/follow/${followedId}`);
        setIsFollowing(res.data.isFollowing);
      } else {
        const res = await api.post(`/follow/${followedId}`);
        setIsFollowing(res.data.isFollowing);
      }
    } catch (err) {
      console.error("Follow action error:", err);

      const message =
        err.response?.data?.message ||
        err.response?.data ||
        "Action failed";

      alert(message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <button
      className={`follow-btn ${isFollowing ? "following" : ""}`}
      onClick={handleFollow}
      disabled={loading}
    >
      {loading ? "..." : isFollowing ? "✓ Following" : "Follow"}
    </button>
  );
}