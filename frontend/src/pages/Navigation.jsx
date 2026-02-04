import { Link } from "react-router-dom";
import "../styles/Homepage.css";

export default function Navbar() {
  return (
    <nav>
      <h2>AHLA BI HA TTALLEH</h2>
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
