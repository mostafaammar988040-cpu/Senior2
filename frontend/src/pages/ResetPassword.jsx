import { useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import api from "../services/api";
import "../styles/Auth.css";

export default function ResetPassword() {
  const { t } = useTranslation();

  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");

  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await api.post("/auth/reset-password", {
        token,
        newPassword: password,
      });

      setMessage(t("reset.success"));
    } catch {
      setMessage(t("reset.error"));
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2>{t("reset.title")}</h2>

        <form onSubmit={handleSubmit}>
          <input
            type="password"
            placeholder={t("reset.placeholder")}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <button type="submit">
            {t("reset.button")}
          </button>
        </form>

        <p>{message}</p>
      </div>
    </div>
  );
}