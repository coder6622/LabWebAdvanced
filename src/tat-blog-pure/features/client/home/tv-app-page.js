import { Component, customElement } from '../../../app/core/component.js'

const TVAppPage = customElement(
  'tv-app-page',
  class extends Component {
    constructor () {
      super()
    }

    render () {
      return `
        <div class="row">
          <h1>Tv app Pages</h1>
          <p>Content Tv Pages</p>
        </div>
      `
    }
  }
)

export default TVAppPage
