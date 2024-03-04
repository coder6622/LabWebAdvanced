import { Component, customElement } from '../../core/component.js'

const LoadingComponent = customElement(
  'loading',
  class extends Component {
    render () {
      return `
      <h1>Loading ...</h1>
      <div class="spinner-grow text-primary" role="status">
      </div>
      <div class="spinner-grow text-secondary" role="status">
      </div>
      <div class="spinner-grow text-success" role="status">
      </div>
      <div class="spinner-grow text-danger" role="status">
      </div>
      <div class="spinner-grow text-warning" role="status">
      </div>
      <div class="spinner-grow text-info" role="status">
      </div>
      <div class="spinner-grow text-light" role="status">
      </div>
      <div class="spinner-grow text-dark" role="status">
      </div> 
    `
    }
  }
)

export default LoadingComponent
