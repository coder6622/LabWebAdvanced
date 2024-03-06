import globalState from '../../../core/GlobalStates.js'
import { Component, customElement } from '../../../core/component.js'

const SideMenu = customElement(
  'side-menu',
  class extends Component {
    constructor () {
      super()
    }

    loginDummy () {
      globalState.setState({ user: { name: 'Long' } })
      const { user } = globalState.getStates(this)

      console.log(user)
    }

    render () {
      return `
        <ul class="d-flex flex-column gap-5 list-inline">
          <li>
            <a href="/" class="navbar-brand">
              Tips & Tricks
            </a>
          </li>
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
      
  `
    }
  }
)
export default SideMenu
