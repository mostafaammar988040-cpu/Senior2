import { useState } from "react";
import api from "../services/api";
import "../styles/Auth.css";

export default function ForgotPassword() {
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    await api.post("/auth/forgot-password", { email });

    setMessage("If your email exists, a reset link has been sent.");
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2>Forgot Password</h2>

        <form onSubmit={handleSubmit}>
          <input
            type="email"
            placeholder="Enter your email"
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <button type="submit">
            Send Reset Link
          </button>
        </form>

        <p>{message}</p>
      </div>
    </div>
  );
}
