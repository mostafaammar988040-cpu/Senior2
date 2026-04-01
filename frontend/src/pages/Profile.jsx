import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Profile.css";
import i18n from "i18next";

export default function Profile() {
  const [user, setUser] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [passwordMode, setPasswordMode] = useState(false);
  const [formData, setFormData] = useState({});
  const [passwordData, setPasswordData] = useState({ currentPassword: "", newPassword: "" });
  const navigate = useNavigate();

  useEffect(() => {
    api.get("/profile/me")
      .then(res => setUser(res.data.user))
      .catch(err => console.log(err));
  }, []);

  if (!user) return <p className="loading">Loading Dashboard...</p>;

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await api.put("/profile/me", formData);
      setUser(res.data.user);
      setEditMode(false);
    } catch (err) {
      console.error(err);
    }
  };

  const handlePasswordSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post("/profile/change-password", passwordData);
      alert("Password updated successfully");
      setPasswordMode(false);
    } catch (err) {
      console.error(err);
      alert("Failed to update password");
    }
  };

  return (
    <section className="profile-page">

<div className="profile-section">
  <h3>🌐 Language</h3>

  <select
    value={i18n.language}
    onChange={(e) => i18n.changeLanguage(e.target.value)}
    className="language-select"
  >
    <option value="en">English</option>
    <option value="ar">العربية</option>
    <option value="fr">Français</option>
  </select>
</div>
      {/* ===== HEADER ===== */}
      <header className="profile-header">
        <div className="profile-info">
          <img
            src={user.profileImageUrl || "/default-avatar.png"}
            alt="profile"
            className="profile-img"
          />
          <div className="profile-text">
            <h1>{user.firstName} {user.lastName}</h1>
            <p className="email">{user.email}</p>
            {user.bio && <p className="bio">{user.bio}</p>}
          </div>
        </div>
        <div className="profile-actions">
          <button onClick={() => setEditMode(true)}>✏️ Edit Profile</button>
          <button onClick={() => setPasswordMode(true)}>🔒 Change Password</button>
        </div>
      </header>

      {/* ===== DASHBOARD GRID ===== */}
      <main className="dashboard-grid">
        <div className="dashboard-card" onClick={() => navigate("/my-trips")}>
          <span className="icon">✈️</span>
          <h2>My Trips</h2>
          <p>View and manage all your travel plans.</p>
        </div>

        <div className="dashboard-card" onClick={() => navigate("/profile/journeys")}>
          <span className="icon">📝</span>
          <h2>My Journeys</h2>
          <p>Track your past and upcoming journeys.</p>
        </div>

        <div className="dashboard-card" onClick={() => navigate("/profile/preferences")}>
          <span className="icon">⚙️</span>
          <h2>Preferences</h2>
          <p>Customize your travel experience.</p>
        </div>

         <div className="dashboard-card" onClick={() => navigate("/profile/favorites")}>
          <span className="icon">❤️</span>
          <h2>Favorites</h2>
          <p>View and manage your favorite destinations.</p>
        </div>


        <div className="dashboard-card" onClick={() => navigate("/profile/suggestions")}>
          <span className="icon">💡</span>
          <h2>Add Suggestion</h2>
          <p>Share ideas to improve the platform.</p>
        </div>
      </main>

      {/* ===== EDIT PROFILE MODAL ===== */}
      {editMode && (
        <div className="modal">
          <form className="modal-content" onSubmit={handleEditSubmit}>
            <h2>Edit Profile</h2>
            <input
              type="text"
              placeholder="First Name"
              defaultValue={user.firstName}
              onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
            />
            <input
              type="text"
              placeholder="Last Name"
              defaultValue={user.lastName}
              onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
            />
            <input
              type="email"
              placeholder="Email"
              defaultValue={user.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
            />
            <textarea
              placeholder="Bio"
              defaultValue={user.bio}
              onChange={(e) => setFormData({ ...formData, bio: e.target.value })}
            />
            <input
              type="text"
              placeholder="Profile Image URL"
              defaultValue={user.profileImageUrl}
              onChange={(e) => setFormData({ ...formData, profileImageUrl: e.target.value })}
            />
            <div className="modal-actions">
              <button type="submit">Save</button>
              <button type="button" onClick={() => setEditMode(false)}>Cancel</button>
            </div>
          </form>
        </div>
      )}

      {/* ===== CHANGE PASSWORD MODAL ===== */}
      {passwordMode && (
        <div className="modal">
          <form className="modal-content" onSubmit={handlePasswordSubmit}>
            <h2>Change Password</h2>
            <input
              type="password"
              placeholder="Current Password"
              onChange={(e) => setPasswordData({ ...passwordData, currentPassword: e.target.value })}
            />
            <input
              type="password"
              placeholder="New Password"
              onChange={(e) => setPasswordData({ ...passwordData, newPassword: e.target.value })}
            />
            <div className="modal-actions">
              <button type="submit">Update</button>
              <button type="button" onClick={() => setPasswordMode(false)}>Cancel</button>
            </div>
          </form>
        </div>
      )}
    </section>
  );
}