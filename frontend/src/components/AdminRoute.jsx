import { Navigate } from "react-router-dom";

export default function AdminRoute({ children }) {

  const token = localStorage.getItem("token");
  const user = JSON.parse(localStorage.getItem("user"));

  // not logged in
  if (!token) {
    return <Navigate to="/login" replace />;
  }

  // logged but not admin
  if (user?.role !== "Admin") {
    return <Navigate to="/" replace />;
  }

  return children;
}