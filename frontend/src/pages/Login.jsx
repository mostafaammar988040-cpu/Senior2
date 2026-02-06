import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

import "../styles/Login.css";

function Login() {
  const [emailOrUsername, setEmailOrUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

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

      navigate("/preferences");
    } catch (error) {
      const message =
        error.response?.data || "Invalid email or password";

      alert(message);
    }
  };

  return (
   <div className="login-page">

  {/* Background */}
  <div className="background"></div>

  <div className="login-wrapper">

    {/* GLASS CARD */}
    <div className="login-card">

      {/* CONTENT */}
      <div className="arch-content">

        <div className="cedar-logo">
          <img src="/images/cedar.png" alt="Cedar  Logo"/>
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
            <span onClick={() => navigate("/signup")}>Sign Up</span>
            <span>Forgot Password?</span>
          </div>

        </form>

      </div>
    </div>
  </div>
</div>
  );
}

export default Login;