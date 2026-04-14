import { useEffect, useState } from "react";
import axios from "axios";
import "./AdPopup.css";

const API_BASE = "https://localhost:7090/api";

export default function AdPopup() {
  const [ad, setAd] = useState(null);
  const [visible, setVisible] = useState(true);
  const [canSkip, setCanSkip] = useState(false);

useEffect(() => {
  const LAST_SEEN_KEY = "lastAdSeen";
  const THIRTY_MIN = 30 * 60 * 1000;

  const now = Date.now();
  const lastSeen = localStorage.getItem(LAST_SEEN_KEY);

  // 🧠 Check if enough time passed
  if (lastSeen && now - Number(lastSeen) < THIRTY_MIN) {
    setVisible(false);
    return;
  }

  const fetchAd = async () => {
    try {
      const { data } = await axios.get(
        "https://localhost:7090/api/Advertisement/active"
      );

      let ads = [];

      if (Array.isArray(data)) ads = data;
      else if (data?.$values) ads = data.$values;

      if (ads.length > 0) {
        setAd(ads[0]);
      } else {
        setVisible(false);
      }
    } catch (err) {
      console.log("Ad popup error:", err);
      setVisible(false);
    }
  };

  fetchAd();

  // ⏱ enable skip after 5 sec
  const timer = setTimeout(() => {
    setCanSkip(true);
  }, 5000);

  return () => clearTimeout(timer);
}, []);

  if (!visible || !ad) return null;

  return (
    <div className="ad-popup-overlay">
      <div className="ad-popup">

        {/* IMAGE */}
      <img
  src={
    ad.imageUrl
      ? `https://localhost:7090${ad.imageUrl}`
      : "https://via.placeholder.com/1200x600"
  }
  alt={ad.placeName}
/>

        {/* TEXT */}
        <div className="ad-content">
          <h2>{ad.placeName}</h2>
          <p>Discover this amazing place now ✨</p>
        </div>

        {/* SKIP BUTTON */}
       <button
  className="skip-btn"
  onClick={() => {
    localStorage.setItem("lastAdSeen", Date.now());
    setVisible(false);
  }}
  disabled={!canSkip}
>
  {canSkip ? "Skip" : "Wait..."}
</button>
      </div>
    </div>
  );
}