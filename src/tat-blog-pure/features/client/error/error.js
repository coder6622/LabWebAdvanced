import { Component, customElement } from '../../../app/core/component.js'

const ErrorPage = customElement(
  'error-page',
  class extends Component {
    render () {
      return `<h1>Error</h1>`
    }
  }
)

export default ErrorPage
