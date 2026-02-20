import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import "../styles/Homepage.css";

export default function Navbar() {
  const navigate = useNavigate();

  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem("token"));

  useEffect(() => {
    const handleStorageChange = () => {
      setIsLoggedIn(!!localStorage.getItem("token"));
    };

    window.addEventListener("storage", handleStorageChange);
    return () => window.removeEventListener("storage", handleStorageChange);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setIsLoggedIn(false);
    navigate("/");
  };

  return (
    <nav>
      <h1>
        <span style={{ color: "#d62828" }}>AHLA</span>{" "}
        <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
        <span style={{ color: "#000000" }}>TALLEH</span>
      </h1>

      <div>
        <Link to="/" style={{ marginLeft: "25px" }}>Home</Link>
        <Link to="/explore" style={{ marginLeft: "25px" }}>Explore</Link>
        <Link to="/events" style={{ marginLeft: "25px" }}>Events</Link>
        <Link to="/ai-assistant" style={{ marginLeft: "25px" }}>AI Assistant</Link>
        <Link to="/taxis" style={{ marginLeft: "25px" }}>Transportation</Link>

        

        <Link to="/SmartItineraryintro" style={{ marginLeft: "25px" }}>
          Smart Itinerary
        </Link>
        {isLoggedIn ? (
          <span
            onClick={handleLogout}
            style={{ cursor: "pointer", marginLeft: "25px" }}
          >
            Logout
          </span>
        ) : (
          <Link to="/login" style={{ marginLeft: "25px" }}>
            Login
          </Link>
        )}
      </div>
    </nav>
  );
}