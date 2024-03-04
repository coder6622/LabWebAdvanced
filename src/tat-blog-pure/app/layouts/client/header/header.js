import { Component, customElement } from '../../../core/component.js'

const HeaderComponent = customElement(
  'header',
  class extends Component {
    render () {
      return `
      <header>
        <nav class="navbar fixed-top navbar-expand-sm
            navbar-toggleable-sm navbar-light
            bg-white border-bottom box-shadow">
          <div class="container-fluid">
            <a href="/" class="navbar-brand">
              Tips & Tricks
            </a>

            <button class="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target=".navbar-collapse"
                    bs-controls="navbarSupportedContent"
                    area-expanded="false"
                    area-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
            </button>

            <div class="navbar-collapse collapse
                d-sm-inline-flex
                justify-content-between">
              <ul class="navbar-nav flex-grow-1">
                <li class="nav-item">
                  <a href='/' class="nav-link text-dark">
                    Trang chủ
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    href="#/contact"
                    title="Xem thông tin liên lạc và góp ý">
                    Liên hệ
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    href='#/about'
                    title="Thông tin về TAT Blog">
                    Giới thiệu
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    title="Tải danh sách bài viết mới">
                    RSS Feed
                  </a>
                </li>
              </ul>
              <ul class="navbar-nav">
                <li class="nav-item">
                  <a href='#/dashboard' class="nav-link text-dark"
                    title="Admin">
                    Admin
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </nav>
      </header>
  `
    }
  }
)
export default HeaderComponent
