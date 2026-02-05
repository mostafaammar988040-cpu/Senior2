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

      {/* HERO */}
      <section className="hero-section">
        <img src={introImg} alt="Discover Lebanon" className="hero-bg" />

        <div className="hero-overlay">
          <h1>Discover Lebanon</h1>
          <p>
            A land where history, culture, and breathtaking nature merge into one unforgettable journey.
          </p>
        </div>
      </section>

      {/* SECTION 1 */}
      <section className="intro-section split">
        <div className="intro-text">
          <h2>Lebanon: A Country of Timeless Beauty</h2>
          <p>
            Lebanon rests along the Mediterranean Sea, offering a unique blend of ancient heritage and modern lifestyle.
            Known for its resilience, warmth, and hospitality.
          </p>
        </div>

        <div className="intro-image">
          <img src={raoucheImg} alt="Raouche Rock" />
        </div>
      </section>

      {/* SECTION 2 */}
      <section className="intro-section split reverse">
        <div className="intro-text">
          <h2>Legacy</h2>
          <p>
            Home to the Phoenicians—masters of trade and navigation—Lebanon helped shape the first alphabet.
          </p>
          <p>
            Cities like Byblos, Baalbek, Sidon, and Tyre preserve remarkable archaeological treasures.
          </p>
        </div>

        <div className="intro-image">
          <img src={ruinsImg} alt="Ancient Ruins" />
        </div>
      </section>

      {/* SECTION 3 */}
      <section className="intro-section split">
        <div className="intro-text">
          <h2>Cultural Diversity</h2>
          <p>
            Lebanon is home to multiple religious and cultural communities living side by side,
            creating a vibrant cultural mosaic.
          </p>
        </div>

        <div className="intro-image">
          <img src={mosqueImg} alt="Mosque" />
        </div>
      </section>

      {/* SECTION 4 */}
      <section className="intro-section split reverse">
        <div className="intro-text">
          <h2>Nature Like Nowhere Else</h2>
          <p>
            Ski in the morning, swim in the afternoon. Beaches, mountains, forests, and waterfalls —
            all in one country.
          </p>
        </div>

        <div className="intro-image grid">
          <img src={nature1Img} alt="Nature View 1" />
          <img src={nature2Img} alt="Nature View 2" />
        </div>
      </section>

      {/* CTA */}
      <div className="intro-cta">
        <button onClick={() => navigate("/")}>
          Start Exploring
        </button>
      </div>

    </div>
  );
}

export default Introduction;
