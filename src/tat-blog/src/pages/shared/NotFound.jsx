import React from 'react';
import { Link } from 'react-router-dom';
import { publicRoutes } from 'config/routes';

export default function NotFound() {
  return (
    <div className='d-flex align-items-center justify-content-center vh-100'>
      <div className='text-center'>
        <h1 className='display-1 fw-bold'>404</h1>
        <p className='fs-3'>
          {' '}
          <span className='text-danger'>Opps!</span>Trang không tìm thấy.
        </p>
        <p className='lead'>Trang mà bạn đang tìm không tồn tại</p>
        <Link
          to={publicRoutes.home}
          className='btn btn-primary'
        >
          Quay về trang chủ
        </Link>
      </div>
    </div>
  );
}
