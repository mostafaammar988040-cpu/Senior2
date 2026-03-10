import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7090/api"
});
console.log('API baseURL:', api.defaults.baseURL);

api.interceptors.request.use(config => {

  const token = localStorage.getItem("token");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default api;