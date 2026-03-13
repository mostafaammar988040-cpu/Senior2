import { useState, useEffect } from "react";
import { Line, Doughnut } from "react-chartjs-2";
import api from "../services/api";
import "../styles/AdminDashboard.css";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement,
  Tooltip,
  Legend,
  Filler
} from "chart.js";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement,
  Tooltip,
  Legend,
  Filler
);

function AdminDashboard() {

  const [metrics, setMetrics] = useState({
    users: 0,
    trips: 0,
    reviews: 0,
    flags: 0
  });

  const [visitsData, setVisitsData] = useState(null);
  const [ratingsData, setRatingsData] = useState(null);

  /* ---------------- LOAD DASHBOARD ---------------- */

  useEffect(() => {

    api.get("/admin/dashboard")
      .then(res => {
        setMetrics(res.data);
      });

    api.get("/admin/dashboard-charts")
      .then(res => {

        const tripDates = res.data.tripsPerDay.map(t =>
          new Date(t.date).toLocaleDateString()
        );

        const tripCounts = res.data.tripsPerDay.map(t => t.count);

        setVisitsData({
          labels: tripDates,
          datasets: [
            {
              label: "Trips Created",
              data: tripCounts,
              borderColor: "#7c5cff",
              backgroundColor: "rgba(124,92,255,0.25)",
              tension: 0.4,
              fill: true
            }
          ]
        });

        const ratingCounts = [0,0,0,0,0];

        res.data.ratings.forEach(r => {
          ratingCounts[r.rating - 1] = r.count;
        });

        setRatingsData({
          labels: ["1★","2★","3★","4★","5★"],
          datasets: [
            {
              data: ratingCounts,
              backgroundColor: [
                "#ff5c7a",
                "#ff884d",
                "#ffcc66",
                "#7c5cff",
                "#31d0aa"
              ]
            }
          ]
        });

      });

  }, []);

  return (
    <div className="dashboard-container">

      <h2 className="dashboard-title">
        <span style={{ color: "#000000" }}>
          Welcome Back Admin
        </span>
      </h2>

      {/* Metrics */}

      <section className="metrics-grid">

        <div className="metric-card">
          <p>Total Users</p>
          <h3>{metrics.users}</h3>
        </div>

        <div className="metric-card">
          <p>Total Trips</p>
          <h3>{metrics.trips}</h3>
        </div>

        <div className="metric-card">
          <p>Total Reviews</p>
          <h3>{metrics.reviews}</h3>
        </div>

        <div className="metric-card">
          <p>Support Requests</p>
          <h3>{metrics.flags}</h3>
        </div>

      </section>

      {/* Charts */}

      <section className="charts-grid">

        <div className="chart-card">
          <h3>Trips Created (Last 7 Days)</h3>

          {visitsData ? (
            <Line data={visitsData} />
          ) : (
            <p>Loading chart...</p>
          )}

        </div>

        <div className="chart-card">
          <h3>Review Rating Distribution</h3>

          {ratingsData ? (
            <Doughnut data={ratingsData} />
          ) : (
            <p>Loading chart...</p>
          )}

        </div>

      </section>

      {/* Alerts */}

      <section className="bottom-grid">

        <div className="alert-card">
          <h3>System Alerts</h3>

          <ul>
            <li>⚠ Monitor user support requests</li>
            <li>🔥 Trip creation activity increasing</li>
            <li>⭐ Review trends updated automatically</li>
          </ul>
        </div>

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