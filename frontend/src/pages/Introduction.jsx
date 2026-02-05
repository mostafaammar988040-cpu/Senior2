import { useNavigate } from "react-router-dom";
import "../styles/Introduction.css";

import introImg from "../assets/background-intro.jpg";
import raoucheImg from "../assets/raouche.jpg";
import ruinsImg from "../assets/ruins.jpg";
import mosqueImg from "../assets/mosque.jpg";
import nature1Img from "../assets/nature1.jpg";
import nature2Img from "../assets/nature2.jpg";

function Introduction() {
  const navigate = useNavigate();

  return (
    <div className="intro-page">

      <section className="hero-section">
        <img
          src={introImg}
          alt="Discover Lebanon"
          className="hero-bg"
        />
        <div className="hero-overlay">
          <h1>Discover Lebanon</h1>
          <p>
            A land where history, culture, and breathtaking nature merge into one unforgettable journey.
          </p>
        </div>
      </section>

      <section className="intro-section split">
        <div className="intro-text">
          <h2>Lebanon: A Country of Timeless Beauty</h2>
          <p>
            Lebanon rests along the Mediterranean Sea, offering a unique blend of ancient heritage and modern lifestyle. Known for its resilience, warmth, and hospitality, Lebanon remains a captivating destination for travelers from around the globe.
          </p>
        </div>

        <div className="intro-image">
          <img src={raoucheImg} alt="Raouche Rock" />
        </div>
      </section>

      <section className="intro-section split reverse">
        <div className="intro-text">
          <h2>Legacy</h2>
          <p>
            Home to the Phoenicians—masters of trade and navigation—Lebanon played a major role in developing the first alphabet. Cities like Byblos, Baalbek, Sidon, and Tyre hold some of the world's most remarkable archaeological sites.
          </p>
          <p>
            Throughout its long history, Lebanon was influenced by civilizations such as the Romans, Arabs, Ottomans, and French, creating a rich cultural mosaic.
          </p>
        </div>

        <div className="intro-image">
          <img src={ruinsImg} alt="Ruins" />
        </div>
      </section>

      <section className="intro-section split">
        <div className="intro-text">
          <h2>Cultural Diversity</h2>
          <p>
            Lebanon is home to multiple religious and cultural communities living side by side. This diversity fuels its vibrant traditions, festivals, music, and world-famous cuisine.
          </p>
        </div>

        <div className="intro-image">
          <img src={mosqueImg} alt="Mosque" />
        </div>
      </section>

      <section className="intro-section split reverse">
        <div className="intro-text">
          <h2>Nature Like Nowhere Else</h2>
          <p>
            Sandy beaches, cedar forests, snowy mountains, cascading waterfalls — Lebanon’s nature is incredibly diverse. Few countries allow you to ski in the morning and swim in the afternoon on the same day.
          </p>
        </div>

        <div className="intro-image grid">
          <img src={nature1Img} alt="Nature 1" />
          <img src={nature2Img} alt="Nature 2" />
        </div>
      </section>

      <div className="intro-cta">
        <button onClick={() => navigate("/")}>
          Start Exploring
        </button>
      </div>

    </div>
  );
}

export default Introduction;
