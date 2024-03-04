import { Component, customElement } from '../../../core/component.js'
import store from '../../../store/index.js'

const MainLayout = customElement(
  'main-layout',
  class extends Component {
    constructor () {
      super({ store, element: document.querySelector('#app') })
    }
    render () {
      const { childrens } = this.props
      // if (store.state.isLogin) {
      //   return `<div class="container-fluid">
      //   <div class="row">
      //     <div class="col-9">
      //       <main id="router-slot" class="layout-page-content">
      //       </main>
      //     </div>
      //     <div class="col-3 border-start">
      //       <button onClick='${() =>
      //         this.hanldeLoginBtn()}' id="login-btn" class="btn btn-info">Handle State</button>
      //       <app-sidebar></app-sidebar>
      //     </div>
      //   </div>
      // </div>`
      // } else {
      return `
      <app-header></app-header> 
      <div class="container-fluid">
        <div class="row">
          <div class="col-9">
            <main id="router-slot" class="layout-page-content">
            ${childrens}
            </main>
          </div>
          <div class="col-3 border-start">
            <button id="login-btn" class="btn btn-info">Handle State</button>
            <app-sidebar></app-sidebar>
          </div>
        </div>
      </div>
    `
    }

    hanldeLoginBtn () {
      // store.dispatch('setLogin', !store.state.isLogin)
    }
  }
)

export default MainLayout
