import { Component, customElement } from '../../../core/component.js'
import HeaderAdmin from '../header/header.js'

const MainLayoutAdmin = customElement(
  'main-layout-admin',
  class extends Component {
    constructor () {
      super()
    }

    render () {
      return `
        <${HeaderAdmin}></${HeaderAdmin}>
        <div class="container-fluid">
            ${childrens}
        </div>
        <footer class="border-top footer text-muted">
          <div class="container-fluid text-center">
            &copy; 2023 - Tips & Tricks Blog
          </div>
        </footer>
      `
    }
  }
)

export default MainLayoutAdmin
