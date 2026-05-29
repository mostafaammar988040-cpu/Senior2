import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import { GoogleLogin } from "@react-oauth/google";
import "../styles/Login.css";
import { useTranslation } from "react-i18next";

function Login() {
  const { t } = useTranslation();
  const [emailOrUsername, setEmailOrUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  // ======================
  // NORMAL LOGIN
  // ======================
  const handleLogin = async (e) => {
    e.preventDefault();

    if (!emailOrUsername || !password) {
      alert(t("login.fillFields"));
      return;
    }

    try {
      const res = await api.post("/auth/login", {
        emailOrUsername,
        password,
      });

      localStorage.setItem("token", res.data.token);
      localStorage.setItem("user", JSON.stringify(res.data.user));

      window.dispatchEvent(new Event("loginChange"));

      const user = res.data.user;

      if (user.role === "Admin") {
        navigate("/admin");
      } else {
        navigate("/preferences");
      }
    } catch (error) {
      const message =
        error.response?.data || t("login.invalid");

      alert(message);
    }
  };

  return (
    <div className="login-page">

      <div className="background"></div>

      <div className="login-wrapper">
        <div className="login-card">

          <div className="arch-content">

            <div className="cedar-logo">
              <img src="/images/cedar.png" alt="Cedar Logo" />
            </div>

            <h1>{t("login.title")}</h1>
            <h4>{t("login.subtitle")}</h4>

            <form onSubmit={handleLogin}>

              <input
                type="text"
                placeholder={t("login.email")}
                value={emailOrUsername}
                onChange={(e) => setEmailOrUsername(e.target.value)}
              />

              <input
                type="password"
                placeholder={t("login.password")}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />

              <button type="submit" className="login-btn">
                {t("login.button")}
              </button>

              <div className="links">
                <span onClick={() => navigate("/signup")}>
                  {t("login.signup")}
                </span>

                <span onClick={() => navigate("/forgot-password")}>
                  {t("login.forgot")}
                </span>
              </div>

            </form>

            {/* GOOGLE LOGIN */}
            <div
              style={{
                marginTop: "20px",
                display: "flex",
                justifyContent: "center"
              }}
            >
              <GoogleLogin
                onSuccess={async (credentialResponse) => {
                  try {
                    const idToken = credentialResponse.credential;

                    const res = await api.post("/auth/google", {
                      idToken: idToken
                    });

                    localStorage.setItem("token", res.data.token);
                    localStorage.setItem("user", JSON.stringify(res.data.user));

                    window.dispatchEvent(new Event("loginChange"));

                    navigate("/preferences");

                  } catch (err) {
                    console.error(err);
                    alert(t("login.googleError"));
                  }
                }}
                onError={() => {
                  alert(t("login.googleError"));
                }}
              />
            </div>

          </div>

        </div>
      </div>
    </div>
  );
}

export default Login;