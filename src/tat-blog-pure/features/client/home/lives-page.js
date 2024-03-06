import { Component, customElement } from '../../../app/core/component.js'

const LivesPage = customElement(
  'live-page',
  class extends Component {
    constructor () {
      super()
    }

    render () {
      return `
        <div class="row">
          <h1>Live Pages</h1>
          <p>Content Live Pages</p>
        </div>
      `
    }
  }
)

export default LivesPage
