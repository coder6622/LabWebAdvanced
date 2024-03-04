import { Component, customElement } from '../../../core/component.js'

const SidebarComponent = customElement(
  'sidebar',
  class extends Component {
    constructor () {
      super()
    }
    render () {
      return `
      <div class="my-4">
        <app-categories></app-categories>
        <app-archives></app-archives>
      </div>
    `
    }
  }
)

export default SidebarComponent
