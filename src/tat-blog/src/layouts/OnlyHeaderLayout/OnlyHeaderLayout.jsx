import React from 'react';
import Navbar from '../components/client/Navbar/Navbar';
import Footer from '../components/client/Footer/Footer';

export default function OnlyHeaderLayout({ children }) {
  return (
    <>
      <Navbar />
      <div className='container-fluid'>
        <div className='row justify-content-center align-items-center g-2'>{children}</div>
      </div>
      <Footer />
    </>
  );
}
