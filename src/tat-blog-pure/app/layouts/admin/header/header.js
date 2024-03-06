import { Component, customElement } from '../../../core/component.js'

const HeaderAdmin = customElement(
  'header-admin',
  class extends Component {
    render () {
      return `
      <header>
        <nav class="navbar fixed-top navbar-expand-sm
            navbar-toggleable-sm navbar-light
            bg-white border-bottom box-shadow">
          <div class="container-fluid">
            <a class="navbar-brand">
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
                  <a class="nav-link text-dark">
                    Chủ đề
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    title="Xem danh sách tác giả">
                    Tác giả
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    title="Xem danh sách thẻ/từ khóa">
                    Thẻ
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    title="Xem danh sách bài viết">
                    Bài viết
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    title="Xem danh sách bình luận">
                    Bình luận
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

export default HeaderAdmin