import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import "../styles/Homepage.css";

export default function Navbar() {
  const navigate = useNavigate();

  const [isLoggedIn, setIsLoggedIn] = useState(
    !!localStorage.getItem("token")
  );

  // Update login state if token changes (optional improvement)
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
        <Link to="/">Home</Link>
        <Link to="/explore">Explore</Link>
        <Link to="/events">Events</Link>
        <Link to="/ai-assistant">AI Assistant</Link>
<<<<<<< HEAD

        {isLoggedIn ? (
          <span
            onClick={handleLogout}
            style={{ cursor: "pointer", marginLeft: "15px" }}
          >
            Logout
          </span>
        ) : (
          <Link to="/login">Login</Link>
        )}
=======
        <Link to="/login">Login</Link>
         <Link to="/SmartItineraryintro">SmartItineraryintro</Link>
        

>>>>>>> b7fbef53bd16cd465c644d9fd340d9c52a20f9cd
      </div>
    </nav>
  );
}
