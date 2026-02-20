import { useState } from "react";
import { useSearchParams } from "react-router-dom";
import api from "../services/api";
import "../styles/Auth.css";

export default function ResetPassword() {
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

      setMessage("Password updated successfully!");
    } catch {
      setMessage("Invalid or expired link.");
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2>Reset Password</h2>

        <form onSubmit={handleSubmit}>
          <input
            type="password"
            placeholder="New Password"
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <button type="submit">
            Reset Password
          </button>
        </form>

        <p>{message}</p>
      </div>
    </div>
  );
}
