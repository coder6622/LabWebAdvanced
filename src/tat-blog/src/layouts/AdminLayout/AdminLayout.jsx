import Navbar from 'layouts/components/admin/Navbar/Navbar';
import React from 'react';

export default function AdminLayout({ children }) {
  return (
    <>
      <Navbar />
      <div className='container-fluid my-3'>{children}</div>
    </>
  );
}
