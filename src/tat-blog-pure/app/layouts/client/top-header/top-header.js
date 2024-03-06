import { Component, customElement } from '../../../core/component.js'

const HeaderTop = customElement(
  'header-top',
  class extends Component {
    constructor () {
      super()
    }

    atTheFirstRender () {
      console.log('header render')
    }

    render () {
      console.log('header-render')
      return `
          <div class="container-fluid">
              <ul class="d-flex justify-content-center list-inline gap-5 pt-5">
                <li class="nav-item">
                  <a href='/' class="nav-link text-dark">
                  Trends
                  </a>
                </li><li class="nav-item">
                  <a href='#/?tab=news' class="nav-link text-dark">
                    News
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    href="#/?tab=app"
                    title="Xem thông tin liên lạc và góp ý">
                    TV App
                  </a>
                </li>
                <li class="nav-item">
                  <a class="nav-link text-dark"
                    href='#/?tab=live'
                    title="Thông tin về TAT Blog">
                    TV Lives
                  </a>
                </li>
              </ul>
            
          </div>
        
      
  `
    }
  }
)
export default HeaderTop
