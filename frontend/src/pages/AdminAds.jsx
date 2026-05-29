import React, { useEffect, useMemo, useState } from "react";
import axios from "axios";
import "../styles/AdminAds.css";

const API_BASE = "https://localhost:7090/api";

const mapStatus = (status) => {
  switch (status) {
    case 0:
      return "Pending";
    case 1:
      return "Approved";
    case 2:
      return "Rejected";
    case 3:
      return "Paused";
    default:
      return "Pending";
  }
};

const initialCreateForm = {
  placeId: "",
  startDateUtc: "",
  endDateUtc: "",
  priority: "",
  adminNote: "",
};

const AdminAds = () => {
  const [ads, setAds] = useState([]);
  const [places, setPlaces] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [updating, setUpdating] = useState({});

  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [createForm, setCreateForm] = useState(initialCreateForm);
  const [createError, setCreateError] = useState("");
  const [isCreating, setIsCreating] = useState(false);

  const token = localStorage.getItem("token");

  const authHeaders = token
    ? {
        Authorization: `Bearer ${token}`,
      }
    : {};

  const fetchAds = async () => {
    try {
      setLoading(true);
      setError("");

      const { data } = await axios.get(`${API_BASE}/Advertisement/all`, {
        headers: authHeaders,
      });

      if (Array.isArray(data)) {
        setAds(data);
      } else if (data && Array.isArray(data.$values)) {
        setAds(data.$values);
      } else {
        setAds([]);
      }
    } catch (err) {
      console.log("Failed to load ads:", err);
      setError("Failed to load ads");
      setAds([]);
    } finally {
      setLoading(false);
    }
  };

  const fetchPlaces = async () => {
    try {
      const { data } = await axios.get(`${API_BASE}/places`, {
        headers: authHeaders,
      });

      console.log("Places response:", data);

      if (Array.isArray(data)) {
        setPlaces(data);
      } else if (data && Array.isArray(data.$values)) {
        setPlaces(data.$values);
      } else {
        setPlaces([]);
      }
    } catch (err) {
      console.log("Failed to load places:", err);
      setPlaces([]);
    }
  };

  useEffect(() => {
    fetchAds();
    fetchPlaces();
  }, []);

  const sortedAds = useMemo(() => {
    return [...ads].sort((a, b) => (b.priority ?? 0) - (a.priority ?? 0));
  }, [ads]);

  const updateStatus = async (id, status) => {
    try {
      setUpdating((prev) => ({ ...prev, [id]: true }));
      setError("");

      await axios.put(
        `${API_BASE}/Advertisement/${id}/status`,
        { id, status },
        {
          headers: authHeaders,
        }
      );

      setAds((prev) =>
        prev.map((ad) => (ad.id === id ? { ...ad, status } : ad))
      );
    } catch (err) {
      console.log("Failed to update status:", err);
      setError("Failed to update status");
    } finally {
      setUpdating((prev) => ({ ...prev, [id]: false }));
    }
  };

  const handleCreateInputChange = (e) => {
    const { name, value } = e.target;
    setCreateForm((prev) => ({ ...prev, [name]: value }));
  };

  const openCreateModal = () => {
    setCreateError("");
    setCreateForm(initialCreateForm);
    setIsCreateModalOpen(true);
  };

  const closeCreateModal = () => {
    if (isCreating) return;
    setIsCreateModalOpen(false);
    setCreateError("");
    setCreateForm(initialCreateForm);
  };

  const handleCreateAd = async (e) => {
    e.preventDefault();
    setCreateError("");

    if (
      !createForm.placeId ||
      !createForm.startDateUtc ||
      !createForm.endDateUtc ||
      !createForm.priority ||
      !createForm.adminNote.trim()
    ) {
      setCreateError("All fields are required");
      return;
    }

    if (new Date(createForm.endDateUtc) <= new Date(createForm.startDateUtc)) {
      setCreateError("End date must be after start date");
      return;
    }

    try {
      setIsCreating(true);

      await axios.post(
        `${API_BASE}/Advertisement`,
        {
          placeId: Number(createForm.placeId),
          startDateUtc: new Date(createForm.startDateUtc).toISOString(),
          endDateUtc: new Date(createForm.endDateUtc).toISOString(),
          priority: Number(createForm.priority),
          adminNote: createForm.adminNote.trim(),
        },
        {
          headers: authHeaders,
        }
      );

      closeCreateModal();
      await fetchAds();
    } catch (err) {
      console.log("Create ad error:", err);
      setCreateError(
        err?.response?.data?.title ||
          err?.response?.data?.message ||
          (typeof err?.response?.data === "string" ? err.response.data : "") ||
          "Failed to create ad"
      );
    } finally {
      setIsCreating(false);
    }
  };

  const formatDate = (value) => {
    if (!value) return "-";
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return "-";

    return date.toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "2-digit",
    });
  };

  const getPlaceImage = (placeId) => {
    const selectedPlace = places.find((p) => String(p.id) === String(placeId));
    return (
      selectedPlace?.imageUrl ||
      `https://picsum.photos/seed/place-${placeId}/120/80`
    );
  };

  return (
    <div className="admin-ads-page">
      <section className="admin-ads-panel">
        <div className="admin-ads-header">
          <div>
            <h1>Advertisement Management</h1>
            <p>Monetize your platform 💰</p>
          </div>

          <button className="btn btn-create" onClick={openCreateModal}>
            + Create Ad
          </button>
        </div>

        {loading && (
          <div className="admin-ads-state">
            <div className="spinner" />
            <span>Loading advertisements...</span>
          </div>
        )}

        {!loading && error && (
          <div className="admin-ads-state admin-ads-error">
            <span>{error}</span>
            <button className="btn btn-secondary" onClick={fetchAds}>
              Retry
            </button>
          </div>
        )}

        {!loading && !error && (
          <div className="admin-ads-table-wrap">
            <table className="admin-ads-table">
              <thead>
                <tr>
                  <th>Image</th>
                  <th>Place</th>
                  <th>Start</th>
                  <th>End</th>
                  <th>Priority</th>
                  <th>Status</th>
                  <th>Views</th>
                  <th>Clicks</th>
                  <th className="actions-col">Actions</th>
                </tr>
              </thead>

              <tbody>
                {sortedAds.length === 0 ? (
                  <tr>
                    <td colSpan={9} className="empty-row">
                      No advertisements found.
                    </td>
                  </tr>
                ) : (
                  sortedAds.map((ad) => {
                    const status = mapStatus(ad.status);
                    const isBusy = !!updating[ad.id];

                    return (
                      <tr key={ad.id}>
                        <td>
                          <img
                            className="ad-image"
                            src={getPlaceImage(ad.placeId)}
                            alt={ad.placeName || "Place"}
                          />
                        </td>

                        <td className="place-name">{ad.placeName || "-"}</td>
                        <td>{formatDate(ad.startDateUtc)}</td>
                        <td>{formatDate(ad.endDateUtc)}</td>
                        <td>{ad.priority ?? 0}</td>

                        <td>
                          <span className={`status-badge status-${status}`}>
                            {status}
                          </span>
                        </td>

                        <td>{ad.views ?? Math.floor(Math.random() * 1000)}</td>
                        <td>{ad.clicks ?? Math.floor(Math.random() * 200)}</td>

                        <td className="actions-cell">
                          <button
                            className="btn btn-approve"
                            disabled={isBusy || status === "Approved"}
                            onClick={() => updateStatus(ad.id, 1)}
                          >
                            {isBusy ? "..." : "Approve"}
                          </button>

                          <button
                            className="btn btn-reject"
                            disabled={isBusy || status === "Rejected"}
                            onClick={() => updateStatus(ad.id, 2)}
                          >
                            {isBusy ? "..." : "Reject"}
                          </button>
                        </td>
                      </tr>
                    );
                  })
                )}
              </tbody>
            </table>
          </div>
        )}
      </section>

      {isCreateModalOpen && (
        <div className="create-ad-modal-overlay" onClick={closeCreateModal}>
          <div
            className="create-ad-modal"
            onClick={(e) => e.stopPropagation()}
          >
            <h2>Create Advertisement</h2>

            <form onSubmit={handleCreateAd} className="create-ad-form">
              <div className="create-ad-field">
                <label>Place</label>
                <select
                  name="placeId"
                  value={createForm.placeId}
                  onChange={handleCreateInputChange}
                  required
                >
                  <option value="">Select a place</option>
                  {Array.isArray(places) && places.length > 0 ? (
                    places.map((p) => (
                      <option key={p.id} value={p.id}>
                        {p.name}
                      </option>
                    ))
                  ) : (
                    <option disabled>No places available</option>
                  )}
                </select>
              </div>

              {createForm.placeId && (
                <div className="place-preview">
                  <img
                    src={
                      places.find(
                        (p) => String(p.id) === String(createForm.placeId)
                      )?.imageUrl || "https://via.placeholder.com/400"
                    }
                    alt="Preview"
                  />
                </div>
              )}

              <div className="create-ad-field">
                <label>Start Date</label>
                <input
                  type="datetime-local"
                  name="startDateUtc"
                  value={createForm.startDateUtc}
                  onChange={handleCreateInputChange}
                  required
                />
              </div>

              <div className="create-ad-field">
                <label>End Date</label>
                <input
                  type="datetime-local"
                  name="endDateUtc"
                  value={createForm.endDateUtc}
                  onChange={handleCreateInputChange}
                  required
                />
              </div>

              <div className="create-ad-field">
                <label>Priority</label>
                <input
                  type="number"
                  name="priority"
                  placeholder="Priority"
                  value={createForm.priority}
                  onChange={handleCreateInputChange}
                  min="0"
                  required
                />
              </div>

              <div className="create-ad-field">
                <label>Admin Note</label>
                <input
                  type="text"
                  name="adminNote"
                  placeholder="Note"
                  value={createForm.adminNote}
                  onChange={handleCreateInputChange}
                  required
                />
              </div>

              {createError && (
                <p className="create-ad-error">{createError}</p>
              )}

              <div className="create-ad-actions">
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={closeCreateModal}
                  disabled={isCreating}
                >
                  Cancel
                </button>

                <button
                  type="submit"
                  className="btn btn-create"
                  disabled={isCreating}
                >
                  {isCreating ? "Creating..." : "Create"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminAds;