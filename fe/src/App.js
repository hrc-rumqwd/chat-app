import { Outlet } from 'react-router-dom';
import './App.css';
import Navbar from './layouts/navbar';
import Footer from './layouts/footer';
import ReactRouters from './react-routers';

export default function App() {
  return (
    <div className="container-fluid vh-100 d-flex flex-column p-0 overflow-hidden">
        <Navbar />
          <main role="main" className="flex-grow-1 overflow-hidden">
            <ReactRouters />
          </main>
        <Footer />
      </div>
  );
};