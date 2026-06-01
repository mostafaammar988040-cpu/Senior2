import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import "../styles/Introduction.css";

import introImg from "../assets/background-intro.jpg";
import raoucheImg from "../assets/raouche.jpg";
import ruinsImg from "../assets/ruins.jpg";
import mosqueImg from "../assets/mosque.jpg";
import nature1Img from "../assets/nature1.jpg";
import nature2Img from "../assets/nature2.jpg";

function Introduction() {
  const { t } = useTranslation();
  const navigate = useNavigate();

  return (
    <div className="intro-page">

      {/* HERO */}
      <section className="hero-section">
        <img src={introImg} alt="Discover Lebanon" className="hero-bg" />

        <div className="hero-overlay">
          <h1>{t("intro.heroTitle")}</h1>
          <p>{t("intro.heroText")}</p>
        </div>
      </section>

      {/* SECTION 1 */}
      <section className="intro-section split">
        <div className="intro-text">
          <h2>{t("intro.section1.title")}</h2>
          <p>{t("intro.section1.text")}</p>
        </div>

        <div className="intro-image">
          <img src={raoucheImg} alt="Raouche Rock" />
        </div>
      </section>

      {/* SECTION 2 */}
      <section className="intro-section split reverse">
        <div className="intro-text">
          <h2>{t("intro.section2.title")}</h2>
          <p>{t("intro.section2.text1")}</p>
          <p>{t("intro.section2.text2")}</p>
        </div>

        <div className="intro-image">
          <img src={ruinsImg} alt="Ancient Ruins" />
        </div>
      </section>

      {/* SECTION 3 */}
      <section className="intro-section split">
        <div className="intro-text">
          <h2>{t("intro.section3.title")}</h2>
          <p>{t("intro.section3.text")}</p>
        </div>

        <div className="intro-image">
          <img src={mosqueImg} alt="Mosque" />
        </div>
      </section>

      {/* SECTION 4 */}
      <section className="intro-section split reverse">
        <div className="intro-text">
          <h2>{t("intro.section4.title")}</h2>
          <p>{t("intro.section4.text")}</p>
        </div>

        <div className="intro-image grid">
          <img src={nature1Img} alt="Nature View 1" />
          <img src={nature2Img} alt="Nature View 2" />
        </div>
      </section>

      {/* CTA */}
      <div className="intro-cta">
        <button onClick={() => navigate("/")}>
          {t("intro.cta")}
        </button>
      </div>

    </div>
  );
}

export default Introduction;