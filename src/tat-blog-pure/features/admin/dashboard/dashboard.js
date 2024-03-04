import { Component, customElement } from '../../../app/core/component.js'
const DashboardComponent = customElement(
  'dashboard',
  class extends Component {
    render () {
      return `
      <div class="page-content-container">
        <div class="row">
          <div class="col-xl-6 p-3">
            <div class="card h-100 card-component weather-widget-wrapper">
              <div class="card-body d-flex justify-content-center flex-column">
                <app-weather-widget></app-weather-widget>
              </div>
            </div>
          </div>
          <div class="col-xl-3 p-3">
            <div class="card h-100 card-component">
              <div class="card-body d-flex justify-content-center flex-column">
                <app-calendar></app-calendar>
              </div>
            </div>
          </div>
          <div class="col-xl-3 p-3">
            <div class="card h-100 card-component">
              <app-note></app-note>
            </div>
          </div>
          <div class="col-xl-3 p-3">
            <div class="card h-100 card-component">
              <div class="card-body d-flex justify-content-center flex-column">
                <app-currency></app-currency>
              </div>
            </div>
          </div>
          <div class="col-xl-6 p-3">
            <div class="card h-100 card-component">
              <div class="card-body d-flex justify-content-center flex-column">
                <app-table-payment></app-table-payment>
              </div>
            </div>
          </div>
          <div class="col-xl-3 p-3">
            <div data-earning="" class="card h-100 card-component">
              <app-earning></app-earning>
            </div>
          </div>
        </div>
        <app-alert></app-alert>
      </div>
    `
    }
  }
)
export default DashboardComponent
