import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/Profile.css";
import i18n from "i18next";
import { useTranslation } from "react-i18next";

export default function Profile() {
  const { t } = useTranslation();

  const [user, setUser] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [passwordMode, setPasswordMode] = useState(false);

  const [followers, setFollowers] = useState([]);
  const [following, setFollowing] = useState([]);

  const [followModalOpen, setFollowModalOpen] = useState(false);
  const [activeFollowTab, setActiveFollowTab] = useState("followers");

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    bio: "",
    profileImageUrl: ""
  });

  const [passwordData, setPasswordData] = useState({
    currentPassword: "",
    newPassword: ""
  });

  const navigate = useNavigate();

  useEffect(() => {
    api.get("/profile/me")
      .then(res => {
        setUser(res.data.user);

        setFormData({
          firstName: res.data.user.firstName || "",
          lastName: res.data.user.lastName || "",
          email: res.data.user.email || "",
          bio: res.data.user.bio || "",
          profileImageUrl: res.data.user.profileImageUrl || ""
        });
      })
      .catch(err => console.log(err));

    api.get("/follow/followers")
      .then(res => setFollowers(res.data))
      .catch(err => console.log("Followers error:", err));

    api.get("/follow/following")
      .then(res => setFollowing(res.data))
      .catch(err => console.log("Following error:", err));
  }, []);

  if (!user) return <p className="loading">{t("profile.loading")}</p>;

  const openFollowModal = (tab) => {
    setActiveFollowTab(tab);
    setFollowModalOpen(true);
  };

  const visibleUsers =
    activeFollowTab === "followers" ? followers : following;

  const handleEditSubmit = async (e) => {
    e.preventDefault();

    try {
      const res = await api.put("/profile/me", formData);
      setUser(res.data.user);
      setEditMode(false);
      alert("Profile updated successfully");
    } catch (err) {
      console.error(err);
      alert("Failed to update profile");
    }
  };

  const handlePasswordSubmit = async (e) => {
    e.preventDefault();

    if (!passwordData.currentPassword || !passwordData.newPassword) {
      alert("Please fill all password fields");
      return;
    }

    try {
      await api.post("/profile/change-password", passwordData);

      alert("Password updated successfully");

      setPasswordData({
        currentPassword: "",
        newPassword: ""
      });

      setPasswordMode(false);
    } catch (err) {
      console.error(err);
      alert("Current password is incorrect or update failed");
    }
  };

  return (
    <section className="db-wrap">

      {/* SIDEBAR */}
      <aside className="db-sidebar">

        <div className="db-avatar-ring">
          <img
            className="db-avatar"
            src={
              user.profileImageUrl ||
              `https://ui-avatars.com/api/?name=${user.firstName}+${user.lastName}`
            }
            alt="profile"
          />
        </div>

        <div className="db-name">{user.firstName} {user.lastName}</div>
        <div className="db-email">{user.email}</div>

        {user.bio && <div className="db-bio">{user.bio}</div>}

        {/* FOLLOW STATS ONLY */}
        <div className="follow-stats">
          <button
            type="button"
            className="follow-stat"
            onClick={() => openFollowModal("followers")}
          >
            <strong>{followers.length}</strong>
            <span>Followers</span>
          </button>

          <button
            type="button"
            className="follow-stat"
            onClick={() => openFollowModal("following")}
          >
            <strong>{following.length}</strong>
            <span>Following</span>
          </button>
        </div>

        <div className="profile-actions">
          <button
            className="profile-btn"
            onClick={() => setEditMode(true)}
          >
            Edit Profile
          </button>

          <button
            className="profile-btn outline"
            onClick={() => setPasswordMode(true)}
          >
            Change Password
          </button>
        </div>

        <div className="db-badge">{t("profile.badge")}</div>
      </aside>

      {/* MAIN */}
      <main className="db-main">

        <div className="profile-section">
          <h3>🌐 {t("profile.language")}</h3>

          <select
            value={i18n.language}
            onChange={(e) => {
              i18n.changeLanguage(e.target.value);
              document.documentElement.dir = e.target.value === "ar" ? "rtl" : "ltr";
            }}
            className="language-select"
          >
            <option value="en">English</option>
            <option value="ar">العربية</option>
            <option value="fr">Français</option>
          </select>
        </div>

        <div className="db-header">
          <div className="db-greeting">{t("profile.welcome")}</div>
          <div className="db-title">
            {t("profile.dashboard")} <span>{t("profile.highlight")}</span>
          </div>
        </div>

        <div className="db-cards">

          <div className="db-card trips" onClick={() => navigate("/my-trips")}>
            <div className="db-card-title">{t("profile.myTrips")}</div>
            <div className="db-card-desc">{t("profile.myTripsDesc")}</div>
          </div>

          <div className="db-card journeys" onClick={() => navigate("/profile/journeys")}>
            <div className="db-card-title">{t("profile.myJourneys")}</div>
            <div className="db-card-desc">{t("profile.myJourneysDesc")}</div>
          </div>

          <div className="db-card prefs" onClick={() => navigate("/profile/preferences")}>
            <div className="db-card-title">{t("profile.preferences")}</div>
            <div className="db-card-desc">{t("profile.preferencesDesc")}</div>
          </div>

          <div className="db-card favorites" onClick={() => navigate("/profile/favorites")}>
            <div className="db-card-title">{t("profile.favorites")}</div>
            <div className="db-card-desc">{t("profile.favoritesDesc")}</div>
          </div>

          <div className="db-card suggest" onClick={() => navigate("/profile/suggestions")}>
            <div className="db-card-title">{t("profile.suggestion")}</div>
            <div className="db-card-desc">{t("profile.suggestionDesc")}</div>
          </div>

        </div>

        <div className="db-banner">
          <div className="db-banner-text">
            <h3>{t("profile.aiTitle")}</h3>
            <p>{t("profile.aiDesc")}</p>
          </div>

          <button
            className="db-banner-btn"
            onClick={() => navigate("/SmartItineraryintro")}
          >
            {t("profile.start")}
          </button>
        </div>

      </main>

      {followModalOpen && (
        <div className="modal">
          <div className="modal-content follow-modal-content">

            <div className="follow-modal-header">
              <h2>
                {activeFollowTab === "followers" ? "Followers" : "Following"}
              </h2>

              <button
                type="button"
                className="follow-modal-close"
                onClick={() => setFollowModalOpen(false)}
              >
                ✕
              </button>
            </div>

            <div className="follow-modal-tabs">
              <button
                type="button"
                className={activeFollowTab === "followers" ? "active" : ""}
                onClick={() => setActiveFollowTab("followers")}
              >
                Followers ({followers.length})
              </button>

              <button
                type="button"
                className={activeFollowTab === "following" ? "active" : ""}
                onClick={() => setActiveFollowTab("following")}
              >
                Following ({following.length})
              </button>
            </div>

            {visibleUsers.length === 0 ? (
              <p className="empty-follow-list">
                {activeFollowTab === "followers"
                  ? "No followers yet."
                  : "You are not following anyone yet."}
              </p>
            ) : (
              <div className="follow-table-wrapper">
                <table className="follow-table">
                  <thead>
                    <tr>
                      <th>User</th>
                      <th>Email</th>
                    </tr>
                  </thead>

                  <tbody>
                    {visibleUsers.map((person) => (
                      <tr key={person.id}>
                        <td>
                          <div className="follow-user-cell">
                            <img
                              src={
                                person.profileImageUrl ||
                                `https://ui-avatars.com/api/?name=${person.firstName}+${person.lastName}`
                              }
                              alt="profile"
                            />

                            <span>
                              {person.firstName} {person.lastName}
                            </span>
                          </div>
                        </td>

                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}

          </div>
        </div>
      )}

      {editMode && (
        <div className="modal">
          <div className="modal-content">
            <h2>Edit Profile</h2>

            <form onSubmit={handleEditSubmit} className="profile-form">

              <input
                type="text"
                placeholder="First Name"
                value={formData.firstName}
                onChange={(e) =>
                  setFormData({ ...formData, firstName: e.target.value })
                }
              />

              <input
                type="text"
                placeholder="Last Name"
                value={formData.lastName}
                onChange={(e) =>
                  setFormData({ ...formData, lastName: e.target.value })
                }
              />

              <input
                type="email"
                placeholder="Email"
                value={formData.email}
                onChange={(e) =>
                  setFormData({ ...formData, email: e.target.value })
                }
              />

              <input
                type="text"
                placeholder="Profile Image URL"
                value={formData.profileImageUrl}
                onChange={(e) =>
                  setFormData({ ...formData, profileImageUrl: e.target.value })
                }
              />

              <textarea
                placeholder="Bio"
                value={formData.bio}
                onChange={(e) =>
                  setFormData({ ...formData, bio: e.target.value })
                }
              />

              <div className="modal-actions">
                <button
                  type="button"
                  className="cancel-btn"
                  onClick={() => setEditMode(false)}
                >
                  Cancel
                </button>

                <button type="submit" className="save-btn">
                  Save Changes
                </button>
              </div>

            </form>
          </div>
        </div>
      )}

      {/* CHANGE PASSWORD MODAL */}
      {passwordMode && (
        <div className="modal">
          <div className="modal-content">
            <h2>Change Password</h2>

            <form onSubmit={handlePasswordSubmit} className="profile-form">

              <input
                type="password"
                placeholder="Current Password"
                value={passwordData.currentPassword}
                onChange={(e) =>
                  setPasswordData({
                    ...passwordData,
                    currentPassword: e.target.value
                  })
                }
              />

              <input
                type="password"
                placeholder="New Password"
                value={passwordData.newPassword}
                onChange={(e) =>
                  setPasswordData({
                    ...passwordData,
                    newPassword: e.target.value
                  })
                }
              />

              <div className="modal-actions">
                <button
                  type="button"
                  className="cancel-btn"
                  onClick={() => setPasswordMode(false)}
                >
                  Cancel
                </button>

                <button type="submit" className="save-btn">
                  Update Password
                </button>
              </div>

            </form>
          </div>
        </div>
      )}

    </section>
  );
}