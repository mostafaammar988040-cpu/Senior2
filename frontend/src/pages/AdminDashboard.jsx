import { Line, Doughnut } from "react-chartjs-2";
import "../styles/AdminDashboard.css";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement,
  Tooltip,
  Legend
} from "chart.js";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement,
  Tooltip,
  Legend
);

function AdminDashboard() {

  /* ---------------- Metrics ---------------- */

  const metrics = {
    users: 1240,
    trips: 318,
    reviews: 4620,
    flags: 27
  };

  /* ---------------- Chart Data ---------------- */

  const visitsData = {
    labels: ["Mon","Tue","Wed","Thu","Fri","Sat","Sun"],
    datasets: [
      {
        label: "Visits",
        data: [820,910,760,980,1120,1400,1250],
        borderColor: "#7c5cff",
        backgroundColor: "rgba(124,92,255,0.2)",
        tension: 0.4,
        fill: true
      }
    ]
  };

  const ratingsData = {
    labels: ["5★","4★","3★","2★","1★"],
    datasets: [
      {
        data: [54,26,10,6,4],
        backgroundColor: [
          "#31d0aa",
          "#7c5cff",
          "#ffcc66",
          "#ff884d",
          "#ff5c7a"
        ]
      }
    ]
  };

  return (
    <div className="dashboard-container">

      <h2 className="dashboard-title">

            <span style={{ color: " #000000"}}> Welcome Back Admin</span>{""} 
            </h2>     

      {/* Metrics */}
      <section className="metrics-grid">

        <div className="metric-card">
          <p>Total Users</p>
          <h3>{metrics.users}</h3>
        </div>

        <div className="metric-card">
          <p>Active Trips</p>
          <h3>{metrics.trips}</h3>
        </div>

        <div className="metric-card">
          <p>Total Reviews</p>
          <h3>{metrics.reviews}</h3>
        </div>

        <div className="metric-card">
          <p>Flags / Reports</p>
          <h3>{metrics.flags}</h3>
        </div>

      </section>

      {/* Charts */}
      <section className="charts-grid">

        <div className="chart-card">
          <h3>Platform Visits</h3>
          <Line data={visitsData} />
        </div>

        <div className="chart-card">
          <h3>Reviews Rating Distribution</h3>
          <Doughnut data={ratingsData} />
        </div>

      </section>

      {/* Alerts + Quick Actions */}
      <section className="bottom-grid">

        {/* Alerts */}
        <div className="alert-card">
          <h3>System Alerts</h3>

          <ul>
            <li>⚠ 12 flagged reviews need moderation</li>
            <li>🔥 Trip creation increased this week</li>
            <li>⭐ Average rating slightly dropped</li>
          </ul>
        </div>

        {/* Quick Actions */}
        <div className="actions-card">
          <h3>Quick Actions</h3>

          <button className="dashboard-btn">
            Moderate Reviews
          </button>

          <button className="dashboard-btn">
            View Users
          </button>

          <button className="dashboard-btn">
            Generate Report
          </button>

        </div>

      </section>

    </div>
  );
}

export default AdminDashboard;
