import React from 'react';
import { Navbar as Nb, Nav, Container } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import myConfig from '../../../../config';

function Navbar() {
  return (
    <Nb
      collapseOnSelect
      expand='lg'
      bg='white'
      variant='light'
      className='border-bottom shadow'
    >
      <Container>
        <Nb.Brand>
          <Link
            to={myConfig.publicRoutes.home}
            className='nav-link text-dark'
          >
            {' '}
            Tips & Tricks
          </Link>
        </Nb.Brand>
        <Nb.Toggle aria-controls='reponsive-navbar' />
        <Nb.Collapse
          id='responsive-navbar-nv'
          className='d-sm-inline-flex justify-content-between'
        >
          <Nav className='mr-auto flex-grow-1 justify-content-between'>
            <div className='d-flex'>
              <Nav.Item>
                <Link
                  to={myConfig.publicRoutes.home}
                  className='nav-link text-dark'
                >
                  Trang chủ
                </Link>
              </Nav.Item>
              <Nav.Item>
                <Link
                  to={myConfig.publicRoutes.about}
                  className='nav-link text-dark'
                >
                  Giới thiệu
                </Link>
              </Nav.Item>
              <Nav.Item>
                <Link
                  to={myConfig.publicRoutes.contact}
                  className='nav-link text-dark'
                >
                  Liên hệ
                </Link>
              </Nav.Item>
              <Nav.Item>
                <Link
                  to={myConfig.publicRoutes.rss}
                  className='nav-link text-dark'
                >
                  Rss Feed
                </Link>
              </Nav.Item>
            </div>
            <div>
              <Nav.Item>
                <Link
                  to={myConfig.privateRoutes.home}
                  className='nav-link text-dark'
                >
                  Admin
                </Link>
              </Nav.Item>
            </div>
          </Nav>
        </Nb.Collapse>
      </Container>
    </Nb>
  );
}

export default Navbar;
