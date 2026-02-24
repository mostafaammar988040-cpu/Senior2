import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api"; // 👈 IMPORTANT
import "../styles/Help.css";

export default function Help() {
  const navigate = useNavigate();

  const [openIndex, setOpenIndex] = useState(null);

  /* ===== SUPPORT FORM STATES ===== */
  const [form, setForm] = useState({
    name: "",
    email: "",
    subject: "",
    message: "",
  });

  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState("");

  const faqs = [
    {
      question: "How do I explore places in Lebanon?",
      answer:
        "Go to the Explore page to discover destinations, landscapes, and curated experiences across Lebanon.",
    },
    {
      question: "How does the AI Assistant work?",
      answer:
        "Our AI assistant gives recommendations and travel guidance focused only on Lebanon.",
    },
    {
      question: "Is AHLA BHAL TALLEH free to use?",
      answer:
        "Yes — browsing, discovering places, and using the AI assistant are completely free.",
    },
    {
      question: "How are places selected?",
      answer:
        "Locations are carefully curated to highlight authentic experiences, nature, and culture.",
    },
    {
      question: "Can I trust the information shown?",
      answer:
        "We continuously review and improve content to help travelers plan safely.",
    },
    {
      question: "I found incorrect information — what should I do?",
      answer:
        "Contact us directly via email and we’ll review and update it quickly.",
    },
    {
      question: "Why may some media load slowly?",
      answer:
        "Large visuals may depend on internet speed. Give it a moment or refresh.",
    },
    {
      question: "Can I suggest new places?",
      answer:
        "Yes — soon you’ll be able to suggest ideas from your profile and send them directly to us.",
    },
  ];

  /* ===== HANDLE FORM INPUT ===== */
  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  /* ===== SEND SUPPORT ===== */
  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      setLoading(true);
      setSuccess("");

      await api.post("/support", form);

      setSuccess("✔ Message sent successfully!");
      setForm({
        name: "",
        email: "",
        subject: "",
        message: "",
      });

    } catch (err) {
      setSuccess("❌ Failed to send message");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="help-page">

      {/* BACK BUTTON */}
      <button className="back-btn" onClick={() => navigate("/")}>
        ← Back Home
      </button>

      {/* HERO */}
      <section className="help-hero">
        <h1>Help Center</h1>
        <p>We’re here to help you explore Lebanon smoothly and confidently.</p>
      </section>

      {/* FLOATING CARDS */}
      <section className="help-cards">
        <div className="help-card">
          <h3>📩 Contact Support</h3>
          <p>Need direct help from our team?</p>
        </div>

        <div className="help-card">
          <h3>❓ FAQ</h3>
          <p>Quick answers to common questions.</p>
        </div>

        <div className="help-card">
          <h3>⚠️ Report Issue</h3>
          <p>Found a problem? Let us know.</p>
        </div>

        <div className="help-card">
          <h3>🤖 AI Assistant</h3>
          <p>Smart travel help powered by AI.</p>
        </div>
      </section>

      {/* FAQ */}
      <section className="faq-section">
        <h2>Frequently Asked Questions</h2>

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

      {/* ===== CONTACT FORM ===== */}
      <section className="contact-section">
        <h2>Contact Support</h2>

        <form className="support-form" onSubmit={handleSubmit}>
          <input
            name="name"
            placeholder="Your Name"
            value={form.name}
            onChange={handleChange}
            required
          />

          <input
            name="email"
            placeholder="Your Email"
            value={form.email}
            onChange={handleChange}
            required
          />

          <input
            name="subject"
            placeholder="Subject"
            value={form.subject}
            onChange={handleChange}
            required
          />

          <textarea
            name="message"
            placeholder="Your Message..."
            value={form.message}
            onChange={handleChange}
            required
          />

          <button type="submit" disabled={loading}>
            {loading ? "Sending..." : "Send Message"}
          </button>
        </form>

        {success && <p className="success-msg">{success}</p>}

        <p className="reply-time">
          Or email us directly: <b>AhlaBhalTalleh@gmail.com</b>
        </p>
      </section>

    </div>
  );
}