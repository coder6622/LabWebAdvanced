import React from 'react';
import { Navbar as Nb, Nav, Container } from 'react-bootstrap';
import { Link } from 'react-router-dom';

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
        <Nb.Brand href='/'>Tips & Tricks</Nb.Brand>
        <Nb.Toggle aria-controls='reponsive-navbar' />
        <Nb.Collapse
          id='responsive-navbar-nv'
          className='d-sm-inline-flex justify-content-between'
        >
          <Nav className='mr-auto flex-grow-1'>
            <Nav.Item>
              <Link
                to='/'
                className='nav-link text-dark'
              >
                Trang chủ
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to='/blog/about'
                className='nav-link text-dark'
              >
                Giới thiệu
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to='/blog/contact'
                className='nav-link text-dark'
              >
                Liên hệ
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link
                to='/blog/rss'
                className='nav-link text-dark'
              >
                Rss Feed
              </Link>
            </Nav.Item>
          </Nav>
        </Nb.Collapse>
      </Container>
    </Nb>
  );
}

export default Navbar;
