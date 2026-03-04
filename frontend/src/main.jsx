import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { GoogleOAuthProvider } from "@react-oauth/google";
import App from "./App";

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <BrowserRouter>
      <GoogleOAuthProvider clientId="983400158533-ue8udqbg4s77tlr41mmv6ai43p36ovqt.apps.googleusercontent.com"
      >
        <App />
      </GoogleOAuthProvider>
    </BrowserRouter>
  </React.StrictMode>
);