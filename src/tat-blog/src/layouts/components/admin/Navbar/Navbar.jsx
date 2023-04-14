import React from 'react';
import { Navbar as Nb, Nav } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import myConfig from '../../../../config';
const Navbar = () => {
  return (
    <Nb
      collapseOnSelect
      expand='sm'
      bg='white'
      variant='light'
      className='border-bottom shadow'
    >
      <div className='container-fluid'>
        <Nb.Brand>
          <Link
            to={myConfig.publicRoutes.home}
            className='nav-link text-dark'
          >
            Tips & Tricks
          </Link>
        </Nb.Brand>
        <Nb.Toggle aria-controls='responsive-navbar-nav' />
        <Nb.Collapse
          id='responsive-navbar-nav'
          className='d-sm-inline-flex
justify-content-between'
        >
          <Nav className='mr-auto flex-grow-1'>
            <Nav.Item>
              <Link
                to={myConfig.privateRoutes.categories}
                className='nav-link text-dark'
              >
                Chủ đề
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to={myConfig.privateRoutes.authors}
                className='nav-link text-dark'
              >
                Tác giả
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to={myConfig.privateRoutes.tags}
                className='nav-link text-dark'
              >
                Thẻ
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to={myConfig.privateRoutes.posts}
                className='nav-link text-dark'
              >
                Bài viết
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to={myConfig.privateRoutes.comments}
                className='nav-link text-dark'
              >
                Bình luận
              </Link>
            </Nav.Item>
          </Nav>
        </Nb.Collapse>
      </div>
    </Nb>
  );
};

export default Navbar;
