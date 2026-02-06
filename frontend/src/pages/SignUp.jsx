import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Login.css";

function SignUp() {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const navigate = useNavigate();

  const handleSignUp = async (e) => {
    e.preventDefault();

    if (!firstName || !lastName || !email || !password || !confirmPassword) {
      alert("Please fill in all fields");
      return;
    }

    if (password !== confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    try {
      await api.post("/auth/register", {
        firstName,
        lastName,
        email,
        password,
      });

      alert("Account created successfully!");
      navigate("/preferences");
    } catch (error) {
      const message =
        error.response?.data || "Something went wrong during signup";

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

          <h2>Create Account</h2>

          <form onSubmit={handleSignUp}>

            <input
              type="text"
              placeholder="First Name"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
            />

            <input
              type="text"
              placeholder="Last Name"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />

            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />

            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            <input
              type="password"
              placeholder="Confirm Password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />

            <button type="submit" className="login-btn">
              Create Account
            </button>

            <div className="links">
              <span onClick={() => navigate("/login")}>
                Already have an account?
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
