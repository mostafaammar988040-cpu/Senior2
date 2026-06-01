import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next"; // ✅ ADD
import api from "../services/api";
import "../styles/Help.css";

export default function Help() {

  const { t } = useTranslation(); // ✅ ADD
  const navigate = useNavigate();

  const [openIndex, setOpenIndex] = useState(null);

  const [form, setForm] = useState({
    name: "",
    email: "",
    subject: "",
    message: "",
  });

  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState("");

  // ✅ TRANSLATED FAQ
  const faqs = [
    {
      question: t("help.faq.q1"),
      answer: t("help.faq.a1"),
    },
    {
      question: t("help.faq.q2"),
      answer: t("help.faq.a2"),
    },
    {
      question: t("help.faq.q3"),
      answer: t("help.faq.a3"),
    },
    {
      question: t("help.faq.q4"),
      answer: t("help.faq.a4"),
    },
    {
      question: t("help.faq.q5"),
      answer: t("help.faq.a5"),
    },
    {
      question: t("help.faq.q6"),
      answer: t("help.faq.a6"),
    },
    {
      question: t("help.faq.q7"),
      answer: t("help.faq.a7"),
    },
    {
      question: t("help.faq.q8"),
      answer: t("help.faq.a8"),
    },
  ];

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      setLoading(true);
      setSuccess("");

      await api.post("/support", form);

      setSuccess(t("help.success"));
      setForm({
        name: "",
        email: "",
        subject: "",
        message: "",
      });

    } catch (err) {
      setSuccess(t("help.error"));
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="help-page">

      {/* BACK */}
      <button className="back-btn" onClick={() => navigate("/")}>
        ← {t("help.back")}
      </button>

      {/* HERO */}
      <section className="help-hero">
        <h1>{t("help.title")}</h1>
        <p>{t("help.subtitle")}</p>
      </section>

      {/* CARDS */}
      <section className="help-cards">

        <div className="help-card">
          <h3>📩 {t("help.cards.contact")}</h3>
          <p>{t("help.cards.contactText")}</p>
        </div>

        <div className="help-card">
          <h3>❓ {t("help.cards.faq")}</h3>
          <p>{t("help.cards.faqText")}</p>
        </div>

        <div className="help-card">
          <h3>⚠️ {t("help.cards.report")}</h3>
          <p>{t("help.cards.reportText")}</p>
        </div>

        <div className="help-card">
          <h3>🤖 {t("help.cards.ai")}</h3>
          <p>{t("help.cards.aiText")}</p>
        </div>

      </section>

      {/* FAQ */}
      <section className="faq-section">

        <h2>{t("help.faq.title")}</h2>

        {faqs.map((faq, index) => (
          <div
            key={index}
            className={`faq-item ${openIndex === index ? "open" : ""}`}
            onClick={() =>
              setOpenIndex(openIndex === index ? null : index)
            }
          >
            <div className="faq-question">
              {faq.question}
              <span>{openIndex === index ? "−" : "+"}</span>
            </div>

            {openIndex === index && (
              <div className="faq-answer">{faq.answer}</div>
            )}
          </div>
        ))}

      </section>

      {/* FORM */}
      <section className="contact-section">

        <h2>{t("help.contactTitle")}</h2>

        <form className="support-form" onSubmit={handleSubmit}>

          <input
            name="name"
            placeholder={t("help.name")}
            value={form.name}
            onChange={handleChange}
            required
          />

          <input
            name="email"
            placeholder={t("help.email")}
            value={form.email}
            onChange={handleChange}
            required
          />

          <input
            name="subject"
            placeholder={t("help.subject")}
            value={form.subject}
            onChange={handleChange}
            required
          />

          <textarea
            name="message"
            placeholder={t("help.message")}
            value={form.message}
            onChange={handleChange}
            required
          />

          <button type="submit" disabled={loading}>
            {loading ? t("help.sending") : t("help.send")}
          </button>

        </form>

        {success && <p className="success-msg">{success}</p>}

        <p className="reply-time">
          {t("help.direct")} <b>ahlabhaltalleh451@gmail.com</b>
        </p>

      </section>

    </div>
  );
}