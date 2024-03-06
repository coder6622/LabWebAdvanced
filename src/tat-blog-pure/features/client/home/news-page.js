import { Component, customElement } from '../../../app/core/component.js'

const NewsPage = customElement(
  'news-page',
  class extends Component {
    constructor () {
      super()
    }

    render () {
      return `
        <div class="row">
          <h1>News Pages</h1>
          <p>Content News Pages</p>
        </div>
      `
    }
  }
)

export default NewsPage
