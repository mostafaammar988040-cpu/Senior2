import { useRef } from "react";
import "../styles/Homepage.css";

import heroImg from "../assets/landscape.webp";
import baalbekImg from "../assets/baalbek.jpg";
import byblosImg from "../assets/byblos.jpg";
import cedarsImg from "../assets/cedars.jpg";

export default function Home() {


  return (
    <>

     

      {/* HERO SECTION */}
      <section className="hero">
        <img src={heroImg} className="hero-bg" alt="Hero Background" />

        <div className="hero-text">
          <h1>Explore Lebanon Like Never Before</h1>
          <p>
            Colorful, modern, and intuitive plan trips with AI, explore hidden
            gems, find events, view interactive maps, and experience Lebanon’s
            beauty.
          </p>

          <button>Start Your Journey</button>
        </div>
      </section>

      {/* STRIP */}
      <div className="strip">
        <div>✨ Smart Itinerary</div>
        <div>🤖 AI Travel Assistant</div>
        <div>🗺️ Interactive Map</div>
        <div>🎉 Live Events</div>
      </div>

      {/* TOP DESTINATIONS */}
      <section className="explore-section">
        <h2>Top Destinations</h2>

        <div className="explore-grid">
          <div className="explore-card">
            <img src={baalbekImg} alt="Baalbek" />
            <h3>Baalbek Temples</h3>
          </div>

          <div className="explore-card">
            <img src={byblosImg} alt="Byblos" />
            <h3>Byblos Old Souks</h3>
          </div>

          <div className="explore-card">
            <img src={cedarsImg} alt="Cedars" />
            <h3>Cedars Forests</h3>
          </div>
        </div>
      </section>
<section className="home-actions">
  <h2>Ready to Explore Lebanon?</h2>

  <div className="home-buttons">
    <button onClick={() => window.location.href = "/explore"}>
      Explore
    </button>

    <button onClick={() => window.location.href = "/help"}>
      Help
    </button>
  </div>
</section>
      <footer>
        ©️ 2026 AHLA BI HA TTALLEH — A Colorful Way to Explore Lebanon
        
      </footer>
      
    </>
  );
}