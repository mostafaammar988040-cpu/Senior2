import { Link } from "react-router-dom";
import "../styles/Homepage.css";

export default function Navbar() {
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
        <Link to="/ai">AI Assistant</Link>
        <Link to="/login">Login</Link>
      </div>
    </nav>
  );
}
