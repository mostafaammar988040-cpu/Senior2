import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import "../styles/Homepage.css";

export default function Navbar() {
  const navigate = useNavigate();

  const [isLoggedIn, setIsLoggedIn] = useState(false);

  // ==========================
  // CHECK LOGIN STATE
  // ==========================
  useEffect(() => {
    const checkLogin = () => {
      setIsLoggedIn(!!localStorage.getItem("token"));
    };

    // first load
    checkLogin();

    // update when login/logout happens
    window.addEventListener("storage", checkLogin);
    window.addEventListener("loginChange", checkLogin);

    return () => {
      window.removeEventListener("storage", checkLogin);
      window.removeEventListener("loginChange", checkLogin);
    };
  }, []);

  // ==========================
  // LOGOUT
  // ==========================
  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    // trigger navbar refresh
    window.dispatchEvent(new Event("loginChange"));

    navigate("/");
  };

  return (
    <nav>
      <h1>
        <span style={{ color: "#d62828" }}>AHLA</span>{" "}
        <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
        <span style={{ color: "#f9f7f7" }}>TALLEH</span>
      </h1>

      <div>
        <Link to="/" style={{ marginLeft: "25px" }}>
          Home
        </Link>

        <Link to="/events" style={{ marginLeft: "25px" }}>
          Events
        </Link>

        <Link to="/ai-assistant" style={{ marginLeft: "25px" }}>
          AI Assistant
        </Link>

        <Link to="/SmartItineraryintro" style={{ marginLeft: "25px" }}>
          Smart Itinerary
        </Link>

        <Link to="/taxis" style={{ marginLeft: "25px" }}>
          Taxi Services
        </Link>

        <Link to="/experiences" style={{ marginLeft: "25px" }}>
          Experiences
        </Link>

        {/* ✅ SHOW ONLY IF LOGGED IN */}
        {isLoggedIn && (
          <Link to="/profile" style={{ marginLeft: "25px" }}>
            Profile
          </Link>
          
        )}
   {isLoggedIn && (
          <Link to="/recommendations" style={{ marginLeft: "25px" }}>
            Recommendations
          </Link>
          
        )}
        {/* LOGIN / LOGOUT */}
        {isLoggedIn ? (
          <span
            onClick={handleLogout}
            style={{
              cursor: "pointer",
              marginLeft: "25px",
              fontWeight: "600"
            }}
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