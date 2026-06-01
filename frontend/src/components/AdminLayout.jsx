import { Link, Outlet, useLocation } from "react-router-dom";
import "../styles/AdminDashboard.css";

function AdminLayout() {

  const location = useLocation();

  const isActive = (path) => {
    return location.pathname === path ? "active" : "";
  };

  return (
    <div className="admin-dashboard">

      <aside className="sidebar">

        <div className="brand">
          <div className="logo"></div>
          <div>
 <h1>
            <span style={{ color: "#d62828" }}>AHLA</span>{" "}
            <span style={{ color: "#0dc052" }}>BHAL</span>{" "}
            <span style={{ color: "#000000" }}>TALLEH</span>
          </h1>            
          <h2>
            <span style={{ color: " #000000"}}> Admin Dashboard</span>{""} 
            </h2>
          </div>
        </div>

        <nav className="nav">
          <Link to="/admin" className={isActive("/admin")}>
            Overview
          </Link>

          <Link to="/admin/reviews" className={isActive("/admin/reviews")}>
            Reviews
          </Link>

          <Link to="/admin/users" className={isActive("/admin/users")}>
            Users
          </Link>
            <Link to="/admin/suggestions" className={isActive("/admin/suggestions")}>
    Suggestions
  </Link>
   <Link to="/admin/trips" className={isActive("/admin/trips")}>
    Trips
  </Link>
  <Link to="/admin/places" className={isActive("/admin/places")}>
  Places
</Link>
<Link to="/admin/manage-places" className={isActive("/admin/manage-places")}>
  Manage Places
</Link>
<Link to="/admin/support" className={isActive("/admin/support")}>
  Users Support
</Link>
<Link to="/admin/ads" className={isActive("/admin/ads")}>
   Ads
</Link>

        </nav>

      </aside>

      {/* Page Content */}
      <main className="main">
        <Outlet />
      </main>

    </div>
  );
}

export default AdminLayout;
