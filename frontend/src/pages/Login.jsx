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

      alert("Login successful!");
navigate("/preferences");
    } catch (error) {
      const message =
        error.response?.data ||
        "Invalid email or password";

      alert(message);
    }
  };

  const handleGoogleLogin = () => {
    alert("Google login will be implemented later");
  };

  const handleSignUp = (e) => {
    e.preventDefault();
    navigate("/signup");
  };

  return (
    <>
      <div className="background"></div>

      <div className="form-container">
        <div className="logo">
          <img src="/images/cedar-icon.png" alt="Logo" />
          <h1>
            <span style={{ color: "#d62828" }}>AHLA</span>{" "}
            <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
            <span style={{ color: "#000000" }}>TALLEH</span>
          </h1>
        </div>

        <form onSubmit={handleLogin}>
          <input
            type="text"
            placeholder="Email or Username"
            className="input"
            value={emailOrUsername}
            onChange={(e) => setEmailOrUsername(e.target.value)}
          />

          <input
            type="password"
            placeholder="Password"
            className="input"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button type="submit" className="submit-btn">
            Log In
          </button>

          <button
            type="button"
            className="google-btn"
            onClick={handleGoogleLogin}
          >
            <img src="/images/google.png" alt="Google" />
            Continue with Google
          </button>

          <p className="already">
            Don&apos;t have an account?{" "}
            <a href="#" onClick={handleSignUp}>
              Sign Up
            </a>
          </p>
        </form>
      </div>
    </>
  );
}

export default Login;
