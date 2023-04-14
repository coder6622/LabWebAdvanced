import React from 'react';
import Navbar from '../components/client/Navbar/Navbar';
import Sidebar from 'layouts/components/client/Sidebar/Sidebar';
import Footer from '../components/client/Footer/Footer';

function DefaultLayout({ children }) {
  return (
    <>
      <Navbar />
      <div className='container-fluid'>
        <div className='row justify-content-center align-items-center g-2'>
          <div className='col-9'>{children}</div>
          <div className='col-3 border-start'>
            <Sidebar />
          </div>
        </div>
      </div>
      <Footer />
    </>
  );
}

export default DefaultLayout;
