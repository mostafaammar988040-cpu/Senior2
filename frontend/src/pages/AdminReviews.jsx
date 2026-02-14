import { useState } from "react";
import "../styles/AdminPages.css";

function AdminReviews() {

  const [search, setSearch] = useState("");

  const [reviews, setReviews] = useState([
    {
      id: 1,
      user: "Maaya Haddad",
      place: "Jeita Grotto",
      rating: 5,
      comment: "Amazing place!"
    },
    {
      id: 2,
      user: "Karim Nasser",
      place: "Byblos",
      rating: 3,
      comment: "Too crowded"
    }
  ]);

  const deleteReview = (id) => {
    setReviews(reviews.filter(r => r.id !== id));
  };

  const filteredReviews = reviews.filter(r =>
    (r.user + r.place + r.comment)
      .toLowerCase()
      .includes(search.toLowerCase())
  );

  return (
    <div className="admin-page">

      <h2>            <span style={{ color: " #000000"}}> Manage Reviews</span>{""} 
</h2>

      <input
        className="search-input"
        placeholder="Search reviews..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      <table className="admin-table">

        <thead>
          <tr>
            <th>User</th>
            <th>Place</th>
            <th>Rating</th>
            <th>Comment</th>
            <th>Action</th>
          </tr>
        </thead>

        <tbody>
          {filteredReviews.map(r => (
            <tr key={r.id}>
              <td>{r.user}</td>
              <td>{r.place}</td>
              <td>⭐ {r.rating}</td>
              <td>{r.comment}</td>
              <td>
                <button
                  className="danger-btn"
                  onClick={() => deleteReview(r.id)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>

      </table>

    </div>
  );
}

export default AdminReviews;
