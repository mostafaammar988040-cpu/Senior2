import React from "react";
import "../styles/Events.css";

const ChromaGrid = ({ filterType = "All" }) => {

  const demo = [
    {
      type: "Tech",
      name: "Lebanon Tech Meetup",
      date: "March 15, 2026",
      location: "Beirut Digital District",
      image: "/images/tech.jpg"
    },
    {
      type: "Sport",
      name: "Beirut Marathon",
      date: "April 10, 2026",
      location: "Beirut Corniche",
      image: "/images/marathon.jpg"
    },
    {
      type: "Music",
      name: "Byblos Music Festival",
      date: "July 20, 2026",
      location: "Byblos",
      image: "/images/music.jpg"
    },
    {
      type: "History",
      name: "Lebanese History Tour",
      date: "May 5, 2026",
      location: "Beirut Museum",
      image: "/images/history.jpg"
    },
    {
      type: "Sport",
      name: "Mountain Hiking Challenge",
      date: "June 12, 2026",
      location: "Mount Lebanon",
      image: "/images/hiking.jpg"
    },
    {
      type: "Tech",
      name: "Startup Pitch Night",
      date: "August 22, 2026",
      location: "Beirut",
      image: "/images/startup.jpg"
    }
  ];

  const data = demo.filter(
    (item) => filterType === "All" || item.type === filterType
  );

  const openMap = (location) => {
    const url = `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(location)}`;
    window.open(url, "_blank", "noopener,noreferrer");
  };

  const bookTicket = (eventName) => {
    alert(`Booking ticket for ${eventName}`);
  };

  return (
    <>
      {data.map((event, i) => (
        <div key={i} className="event-card">

          <div className="card-image">
            <img src={event.image} alt={event.name} />
          </div>

          <div className="card-content">

            <h2>{event.name}</h2>

            <p><strong>Date:</strong> {event.date}</p>
            <p><strong>Location:</strong> {event.location}</p>

            <button
              className="ticket-btn"
              onClick={() => bookTicket(event.name)}
            >
              Book Tickets 🎫
            </button>

            <button
              className="map-btn"
              onClick={() => openMap(event.location)}
            >
              Open in Google Maps 📍
            </button>

          </div>

        </div>
      ))}
    </>
  );
};

export default ChromaGrid;
