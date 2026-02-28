import { Routes, Route } from "react-router-dom";
import Homepage from "./pages/Homepage";
import Login from "./pages/Login";
import SignUp from "./pages/SignUp";
import AdminLayout from "./components/AdminLayout";
import AdminDashboard from "./pages/AdminDashboard";
import AdminReviews from "./pages/AdminReviews";
import AdminUsers from "./pages/AdminUsers";
import Preferences from "./pages/Preferences";
import Introduction from "./pages/Introduction";
import TaxiServices from "./pages/TaxiServices";


import AIAssistant from "./pages/ai-assistant/AIAssistant";
import Events from "./pages/Events";
import SmartItineraryintro from "./pages/SmartItineraryintro";

import SmartItineraryForm from "./pages/SmartItineraryForm";
import ResetPassword from "./pages/ResetPassword";
import ForgotPassword from "./pages/ForgotPassword";
import Experiences from "./pages/Experiences";
import Explore from "./pages/Explore";
import ActivityTypes from "./pages/Activitytypes";
import Activities from "./pages/Activities";
import Help from "./pages/Help";
import Places from "./pages/Places";
import Profile from "./pages/Profile";
import PrivateRoute from "./components/PrivateRoute";
function App() {
  return (
    <Routes>
      <Route path="/" element={<Homepage />} />
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignUp />} />

      <Route path="/admin" element={<AdminLayout />}>
        <Route index element={<AdminDashboard />} />
        <Route path="reviews" element={<AdminReviews />} />
        <Route path="users" element={<AdminUsers />} />
      </Route>

      <Route path="/preferences" element={<Preferences />} />
      <Route path="/introduction" element={<Introduction />} />
      <Route path="/taxis" element={<TaxiServices />} />
      <Route path="/preferences" element={<Preferences />} />
      <Route path="/introduction" element={<Introduction />} />
      <Route path="/ai-assistant" element={<AIAssistant/>}/>
      <Route path="/reset-password" element={<ResetPassword />} />
      <Route path="/forgot-password" element={<ForgotPassword />} />
      <Route path="/experiences" element={<Experiences />} />
      <Route path="/explore" element={<Explore />} />
      <Route path="/activities" element={<Activities />} />
      <Route path="events" element={<Events />} />
      <Route path="/SmartItineraryintro" element={<SmartItineraryintro />} />
      <Route path="/SmartItinerary" element={<SmartItineraryForm />} />
      <Route path="/help" element={<Help />} />
      <Route path="/places" element={<Places />} />
      <Route
  path="/profile"
  element={
    <PrivateRoute>
      <Profile />
    </PrivateRoute>
  }
/>
    </Routes>

  );
}

export default App;
