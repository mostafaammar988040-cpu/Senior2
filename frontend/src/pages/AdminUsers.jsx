import { useState } from "react";
import "../styles/AdminPages.css";

function AdminUsers() {

  const [search, setSearch] = useState("");

  const [users, setUsers] = useState([
    {
      id: 1,
      name: "Mostafa Ammar",
      email: "mostafa@example.com",
      blocked: false
    },
    {
      id: 2,
      name: "Rita Abou",
      email: "rita@example.com",
      blocked: true
    }
  ]);

  const toggleBlock = (id) => {
    setUsers(users.map(u =>
      u.id === id ? { ...u, blocked: !u.blocked } : u
    ));
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
                  onClick={() => toggleBlock(u.id)}
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
