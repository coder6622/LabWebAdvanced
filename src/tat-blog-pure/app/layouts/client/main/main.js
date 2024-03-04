import { Component, customElement } from '../../../core/component.js'
import HeaderComponent from '../header/header.js'
import SidebarComponent from '../sidebar/sidebar.js'

const MainLayout = customElement(
  'main-layout',
  class extends Component {
    constructor () {
      super()
    }
    render () {
      const { childrens } = this.props
      return `
        <${HeaderComponent}></${HeaderComponent}> 
        <div class="container-fluid">
          <div class="row">
            <div class="col-9">
              <main id="router-slot" class="layout-page-content">
              ${childrens}
              </main>
            </div>
            <div class="col-3 border-start">
              <${SidebarComponent}></${SidebarComponent}>
            </div>
          </div>
        </div>
      `
    }
  }
)

export default MainLayout
