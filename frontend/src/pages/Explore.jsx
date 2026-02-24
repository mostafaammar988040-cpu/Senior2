import { useRef } from "react";
import { useNavigate } from "react-router-dom";
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
  const videoRef = useRef(null);
  const navigate = useNavigate();

  const handleFullScreen = () => {
    if (videoRef.current) {
      videoRef.current.requestFullscreen();
    }
  };

  const images = [
    { img: img1, title: "Beirut Skyline" },
    { img: img2, title: "Mountain Villages" },
    { img: img3, title: "Baalbek Ruins" },
    { img: img4, title: "Coastal Views" },
    { img: img5, title: "Hidden Caves" },
    { img: img6, title: "Old Souks" },
    { img: img7, title: "Nature Trails" },
    { img: img8, title: "Snow Mountains" },
    { img: img9, title: "Sunset Peaks" },
    { img: img10, title: "Winter Lebanon" },
    { img: img11, title: "Raoche Lebanon" },
    { img: img12, title: "hikes Lebanon" },

  ];

  return (
    <div className="explore-page">

      {/* BACK BUTTON */}
      <button className="back-home" onClick={() => navigate("/")}>
        ← Back to Home
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
          <h1>Explore Lebanon</h1>
          <p>Click anywhere to watch fullscreen ✨</p>
        </div>
      </section>

      {/* GALLERY */}
      <section className="lebanon-gallery">
        <h2>Discover Beautiful Lebanon</h2>

        <div className="gallery-grid">
          {images.map((item, index) => (
            <div className="gallery-card" key={index}>
              <img src={item.img} alt={item.title} />
              <div className="gallery-overlay">
                <h3>{item.title}</h3>
              </div>
            </div>
          ))}
        </div>
      </section>

    </div>
  );
}