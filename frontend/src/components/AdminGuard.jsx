import { Navigate } from "react-router-dom";

export default function AdminGuard({ children }) {

  const user = JSON.parse(localStorage.getItem("user"));

  // if not logged or not admin
  if (!user || user.role !== "Admin") {
    return <Navigate to="/admin/login" replace />;
  }

  return children;
}