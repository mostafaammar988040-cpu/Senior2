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
        error.response?.data ||
        "Something went wrong during signup";

      alert(message);
    }
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

        <form onSubmit={handleSignUp}>
          <input
            type="text"
            placeholder="First Name"
            className="input"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
          />

          <input
            type="text"
            placeholder="Last Name"
            className="input"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
          />

          <input
            type="email"
            placeholder="Email"
            className="input"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <input
            type="password"
            placeholder="Password"
            className="input"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <input
            type="password"
            placeholder="Confirm Password"
            className="input"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />

          <button type="submit" className="submit-btn">
            Create Account
          </button>

          <p className="already">
            Already have an account? <a href="/">Log In</a>
          </p>
        </form>
      </div>
    </>
  );
}

export default SignUp;
