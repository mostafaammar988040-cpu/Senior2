import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Login.css";
import { useTranslation } from "react-i18next";

function SignUp() {
  const { t } = useTranslation();

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const navigate = useNavigate();

  const handleSignUp = async (e) => {
    e.preventDefault();

    if (!firstName || !lastName || !email || !password || !confirmPassword) {
      alert(t("signup.fillFields"));
      return;
    }

    if (password !== confirmPassword) {
      alert(t("signup.passwordMismatch"));
      return;
    }

    try {
      await api.post("/auth/register", {
        firstName,
        lastName,
        email,
        password,
      });

      alert(t("signup.success"));
      navigate("/login");
    } catch (error) {
      const message =
        error.response?.data || t("signup.error");

      alert(message);
    }
  };

  return (
    <div className="login-page">

      {/* Background */}
      <div className="background"></div>

      <div className="login-wrapper">

        {/* CARD */}
        <div className="login-card">

          <div className="arch-content">

            <div className="cedar-logo">
              <img src="/images/cedar.png" alt="Cedar Logo" />
            </div>

            <h2>{t("signup.title")}</h2>

            <form onSubmit={handleSignUp}>

              <input
                type="text"
                placeholder={t("signup.firstName")}
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
              />

              <input
                type="text"
                placeholder={t("signup.lastName")}
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
              />

              <input
                type="email"
                placeholder={t("signup.email")}
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />

              <input
                type="password"
                placeholder={t("signup.password")}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />

              <input
                type="password"
                placeholder={t("signup.confirmPassword")}
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
              />

              <button type="submit" className="login-btn">
                {t("signup.button")}
              </button>

              <div className="links">
                <span onClick={() => navigate("/login")}>
                  {t("signup.haveAccount")}
                </span>
              </div>

            </form>

          </div>
        </div>
      </div>
    </div>
  );
}

export default SignUp;