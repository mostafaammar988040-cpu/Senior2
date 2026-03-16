import { useEffect, useState } from "react";
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

export default function AdminReports() {

  const [report, setReport] = useState(null);
  const [sending, setSending] = useState(false);

  const [visitsData, setVisitsData] = useState(null);
  const [ratingsData, setRatingsData] = useState(null);

  useEffect(() => {

    // Report numbers
    api.get("/admin/report")
      .then(res => setReport(res.data))
      .catch(err => console.log(err));

    // Charts
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

  const sendWarnings = async () => {
    try {

      setSending(true);

      const res = await api.post("/admin/report/send-warnings");

      alert(res.data.message);

    } catch (err) {

      console.error(err);
      alert("Failed to send warning emails");

    } finally {

      setSending(false);

    }
  };

  if (!report) return <p>Loading report...</p>;

  return (
    <div className="dashboard-container">

      <h2>📊 Platform Report</h2>

      {/* Metrics */}

      <section className="metrics-grid">

        <div className="metric-card">
          <p>Total Users</p>
          <h3>{report.users}</h3>
        </div>

        <div className="metric-card">
          <p>Total Trips</p>
          <h3>{report.trips}</h3>
        </div>

        <div className="metric-card">
          <p>Total Reviews</p>
          <h3>{report.reviews}</h3>
        </div>

        <div className="metric-card">
          <p>Total Suggestions</p>
          <h3>{report.suggestions}</h3>
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

      {/* Send warnings */}

      <div style={{ marginTop: "40px" }}>

        <button
          className="dashboard-btn"
          onClick={sendWarnings}
          disabled={sending}
        >
          {sending ? "Sending..." : "Send Warning Emails"}
        </button>

      </div>

    </div>
  );
}
