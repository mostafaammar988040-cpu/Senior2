import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import { GoogleLogin } from "@react-oauth/google";
import "../styles/Login.css";

function Login() {
  const [emailOrUsername, setEmailOrUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  // ======================
  // NORMAL LOGIN
  // ======================
  const handleLogin = async (e) => {
    e.preventDefault();

    if (!emailOrUsername || !password) {
      alert("Please fill in all fields");
      return;
    }

    try {
      const res = await api.post("/auth/login", {
        emailOrUsername,
        password,
      });

      localStorage.setItem("token", res.data.token);
      localStorage.setItem("user", JSON.stringify(res.data.user));

      // update navbar instantly
      window.dispatchEvent(new Event("loginChange"));

      navigate("/preferences");
    } catch (error) {
      const message =
        error.response?.data || "Invalid email or password";

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

            <h1>Welcome to Lebanon</h1>
            <h4>Discover the beauty of the Middle East</h4>

            <form onSubmit={handleLogin}>

              <input
                type="text"
                placeholder="Email or Username"
                value={emailOrUsername}
                onChange={(e) => setEmailOrUsername(e.target.value)}
              />

              <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />

              <button type="submit" className="login-btn">
                Login
              </button>

              <div className="links">
                <span onClick={() => navigate("/signup")}>
                  Sign Up
                </span>

                <span onClick={() => navigate("/forgot-password")}>
                  Forgot Password?
                </span>
              </div>

            </form>
<div
  style={{
    marginTop: "20px",
    display: "flex",
    justifyContent: "center"
  }}
><GoogleLogin
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
      alert("Google login failed");
    }
  }}
  onError={() => {
    alert("Google login failed");
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