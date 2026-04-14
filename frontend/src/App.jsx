import { Routes, Route } from "react-router-dom";
import Layout from "./components/Layout"; // 👈 import the new layout
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
import MyTrips from "./pages/MyTrips";
import Journey from "./pages/Journey";
import JourneyEditor from "./pages/JourneyEditor";
import SuggestionPage from "./pages/SuggestionPage";
import AdminSuggestions from "./pages/AdminSuggestions";
import AdminTrips from "./pages/AdminTrips";
import Recommendations from "./pages/Recommendations";
import Notifications from "./pages/Notifications";
import AdminRoute from "./components/AdminRoute";
import AdminReports from "./pages/AdminReports";
  import Feed from "./pages/Feed";
import CreateJourney from "./pages/CreateJourney";
import ProfilePreference from "./components/profile/ProfilePreference";
import ProfileFavorites from "./components/profile/ProfileFavorites";
import AdminPlaces from "./pages/AdminPlaces";
import AdminPlacesList from "./pages/AdminPlacesList";
import AdminSupport from "./pages/AdminSupport";
import AdminAds from "./pages/AdminAds";
import "./i18n";
function App() {
  return (
    <Routes>
      {/* Public routes WITHOUT navbar */}
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignUp />} />
      <Route path="/reset-password" element={<ResetPassword />} />
      <Route path="/forgot-password" element={<ForgotPassword />} />

      {/* Admin routes (already have AdminLayout) */}
<Route
  path="/admin"
  element={
    <AdminRoute>
      <AdminLayout />
    </AdminRoute>
  }
>        <Route index element={<AdminDashboard />} />
        <Route path="reviews" element={<AdminReviews />} />
        <Route path="users" element={<AdminUsers />} />
        <Route path="suggestions" element={<AdminSuggestions />} />
        <Route path="trips" element={<AdminTrips />} />
        <Route path="reports" element={<AdminReports />} />
        <Route path="/admin/places" element={<AdminPlaces />} />
        <Route path="/admin/manage-places" element={<AdminPlacesList />} />
          <Route path="/admin/support" element={<AdminSupport />} />
          <Route path="/admin/ads" element={<AdminAds />} />

      </Route>

      {/* Main app routes WITH navbar (using Layout) */}
      <Route element={<Layout />}>
        <Route path="/" element={<Homepage />} />
        <Route path="/preferences" element={<Preferences />} />
        <Route path="/introduction" element={<Introduction />} />
        <Route path="/taxis" element={<TaxiServices />} />
        <Route path="/ai-assistant" element={<AIAssistant />} />
        <Route path="/experiences" element={<Experiences />} />
        <Route path="/explore" element={<Explore />} />
        <Route path="/activities" element={<Activities />} />
        <Route path="/events" element={<Events />} />
        <Route path="/SmartItineraryintro" element={<SmartItineraryintro />} />
        <Route path="/SmartItinerary" element={<SmartItineraryForm />} />
        <Route path="/help" element={<Help />} />
        <Route path="/places" element={<Places />} />
        <Route path="/my-trips" element={<MyTrips />} />
        <Route path="/profile/journeys" element={<Journey />} />
        <Route path="/profile/journey/new" element={<JourneyEditor />} />
        <Route path="/profile/suggestions" element={<SuggestionPage />} />
        <Route path="/recommendations" element={<Recommendations />} />
        <Route path="/notifications" element={<Notifications />} />
<Route path="/feed" element={<Feed />} />
<Route path="/create-journey-entry" element={<CreateJourney />} />
<Route path="/profile/preferences" element={<ProfilePreference />} />     
<Route path="/profile/favorites" element={<ProfileFavorites />} />
   <Route
          path="/profile"
          element={
            <PrivateRoute>
              <Profile />
            </PrivateRoute>
          }
        />
      </Route>
    </Routes>
  );
}

export default App;