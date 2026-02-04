import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7090/api",
  headers: {
    "Content-Type": "application/json",
  },
});

// Automatically attach JWT token if exists
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
