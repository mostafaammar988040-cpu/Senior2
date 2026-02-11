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
import AIAssistant from "./pages/ai-assistant/AIAssistant";


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
<Route path="/ai-assistant"element={<AIAssistant/>}/>
    </Routes>
  );
}

export default App;
