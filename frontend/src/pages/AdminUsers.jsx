import { useState, useEffect  } from "react";
import api from "../services/api";
import "../styles/AdminPages.css";

function AdminUsers() {

  const [search, setSearch] = useState("");

 const [users, setUsers] = useState([]);
useEffect(() => {

  api.get("/admin/users")
    .then(res => {

      const formatted = res.data.map(u => ({
        id: u.id,
        name: u.name,
        email: u.email,
        blocked: u.blocked
      }));

      setUsers(formatted);

    })
    .catch(err => {
      console.error("Failed to load users", err);
    });

}, []);
  const toggleBlock = async (id, blocked) => {

  try {

    if (blocked) {
      await api.put(`/admin/unblock-user/${id}`);
    } else {
      await api.put(`/admin/block-user/${id}`);
    }

    setUsers(users.map(u =>
      u.id === id ? { ...u, blocked: !u.blocked } : u
    ));

  } catch (err) {
    console.error(err);
  }

};

  const filteredUsers = users.filter(u =>
    (u.name + u.email)
      .toLowerCase()
      .includes(search.toLowerCase())
  );

  return (
    <div className="admin-page">

      <h2>  <span style={{ color: " #000000"}}> User Management</span>{""} </h2>

      <input
        className="search-input"
        placeholder="Search users..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      <table className="admin-table">

        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Status</th>
            <th>Action</th>
          </tr>
        </thead>

        <tbody>
          {filteredUsers.map(u => (
            <tr key={u.id}>
              <td>{u.name}</td>
              <td>{u.email}</td>
              <td>{u.blocked ? "Blocked" : "Active"}</td>
              <td>
                <button
                  className="danger-btn"
onClick={() => toggleBlock(u.id, u.blocked)}
                >
                  {u.blocked ? "Unblock" : "Block"}
                </button>
              </td>
            </tr>
          ))}
        </tbody>

      </table>

    </div>
  );
}

export default AdminUsers;
