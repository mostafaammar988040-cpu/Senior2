import { useState, useEffect } from "react";
import api from "../services/api";
import "../styles/AdminPages.css";

function AdminReviews() {

  const [search, setSearch] = useState("");
  const [reviews, setReviews] = useState([]);

  // Load reviews from backend
  useEffect(() => {

    api.get("/reviews")
      .then(res => {
        setReviews(res.data);
      })
      .catch(err => {
        console.error("Failed to load reviews", err);
      });

  }, []);

  // Delete review
  const deleteReview = async (id) => {

    try {

      await api.delete(`/reviews/${id}`);

      setReviews(reviews.filter(r => r.id !== id));

    } catch (err) {

      console.error("Failed to delete review", err);

    }

  };

  // Filter search
  const filteredReviews = reviews.filter(r =>
    (r.user + r.place + r.comment)
      .toLowerCase()
      .includes(search.toLowerCase())
  );

  return (
    <div className="admin-page">

      <h2>
        <span style={{ color: "#000000" }}>
          Manage Reviews
        </span>
      </h2>

      {/* Search */}

      <input
        className="search-input"
        placeholder="Search reviews..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      {/* Reviews table */}

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

          {filteredReviews.length === 0 ? (
            <tr>
              <td colSpan="5">No reviews found</td>
            </tr>
          ) : (

            filteredReviews.map(r => (

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

            ))

          )}

        </tbody>

      </table>

    </div>
  );
}

export default AdminReviews;
