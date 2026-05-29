import { useState } from "react";
import api from "../services/api";
import "../styles/Auth.css";
import { useTranslation } from "react-i18next";

export default function ForgotPassword() {
  const { t } = useTranslation();

  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    await api.post("/auth/forgot-password", { email });

    setMessage(t("forgot.success"));
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2>{t("forgot.title")}</h2>

        <form onSubmit={handleSubmit}>
          <input
            type="email"
            placeholder={t("forgot.email")}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <button type="submit">
            {t("forgot.button")}
          </button>
        </form>

        <p>{message}</p>
      </div>
    </div>
  );
}