import { useRef } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useGlobalMedia } from "../context/GlobalMediaContext";
import "../styles/Explore.css";

import img1 from "../assets/lebanon1.jpg";
import img2 from "../assets/lebanon2.jpg";
import img3 from "../assets/lebanon3.jpg";
import img4 from "../assets/lebanon4.jpg";
import img5 from "../assets/lebanon5.jpg";
import img6 from "../assets/lebanon6.jpg";
import img7 from "../assets/lebanon7.jpg";
import img8 from "../assets/lebanon8.jpg";
import img9 from "../assets/lebanon9.jpg";
import img10 from "../assets/lebanon10.jpg";
import img11 from "../assets/lebanon11.jpg";
import img12 from "../assets/lebanon12.jpg";

export default function Explore() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const videoRef = useRef(null);

  const { playMusic, toggleMusic, isPlaying } = useGlobalMedia();

  const handleFullScreen = () => {
    if (videoRef.current) {
      videoRef.current.requestFullscreen();
    }
  };

  const handlePlaySong = (e) => {
    e.stopPropagation();
    playMusic();
  };

  const images = [
    { img: img1, key: "beirut" },
    { img: img2, key: "mountains" },
    { img: img3, key: "baalbek" },
    { img: img4, key: "coast" },
    { img: img5, key: "caves" },
    { img: img6, key: "souks" },
    { img: img7, key: "trails" },
    { img: img8, key: "snow" },
    { img: img9, key: "sunset" },
    { img: img10, key: "winter" },
    { img: img11, key: "raouche" },
    { img: img12, key: "hiking" },
  ];

  return (
    <div className="explore-page">
      {/* BACK BUTTON */}
      <button className="back-home" onClick={() => navigate("/")}>
        ← {t("explore.back")}
      </button>

      {/* VIDEO HERO */}
      <section className="video-hero" onClick={handleFullScreen}>
        <video
          ref={videoRef}
          className="video-bg"
          src="/images/leb.mp4"
          autoPlay
          muted
          loop
          playsInline
        />

        <div className="video-overlay-text">
          <h1>{t("explore.title")}</h1>
          <p>{t("explore.fullscreen")}</p>

          <button className="play-song-btn" onClick={handlePlaySong}>
            {isPlaying ? "Music Playing 🎵" : "Play Music 🎵"}
          </button>

          <button
            className="pause-song-btn"
            onClick={(e) => {
              e.stopPropagation();
              toggleMusic();
            }}
          >
            {isPlaying ? "Pause Music" : "Resume Music"}
          </button>
        </div>
      </section>

      {/* GALLERY */}
      <section className="lebanon-gallery">
        <h2>{t("explore.galleryTitle")}</h2>

        <div className="gallery-grid">
          {images.map((item, index) => (
            <div className="gallery-card" key={index}>
              <img src={item.img} alt={t(`explore.images.${item.key}`)} />

              <div className="gallery-overlay">
                <h3>{t(`explore.images.${item.key}`)}</h3>
              </div>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}