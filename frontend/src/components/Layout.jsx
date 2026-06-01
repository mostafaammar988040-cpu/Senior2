import { Outlet } from 'react-router-dom';
import Navigation from '../pages/Navigation'; // adjust path if needed

const Layout = () => {
  return (
    <>
      <Navigation />
      <Outlet /> {/* This renders the child route component */}
    </>
  );
};

export default Layout;