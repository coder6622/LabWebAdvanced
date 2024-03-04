import { Component, customElement } from '../../../app/core/component.js'

const AboutPage = customElement(
  'about-page',
  class extends Component {
    render () {
      return `
<div class="container-fluid">
  <div class="row">
    <div class="col-sm-8">
      <h2>About Blog Page</h2>
      <h4>Lorem ipsum..</h4>
      <p>Lorem ipsum..</p>
      <button class="btn btn-default btn-lg">Get in Touch</button>
    </div>
    <div class="col-sm-4">
      <span class="glyphicon glyphicon-signal logo"></span>
    </div>
  </div>
</div>

<div class="container-fluid bg-grey">
  <div class="row">
    <div class="col-sm-4">
      <span class="glyphicon glyphicon-globe logo"></span>
    </div>
    <div class="col-sm-8">
      <h2>Our Values</h2>
      <h4><strong>MISSION:</strong> Our mission lorem ipsum..</h4>
      <p><strong>VISION:</strong> Our vision Lorem ipsum..</p>
    </div>
  </div>
</div>
    `
    }
  }
)

export default AboutPage
