import { Component, customElement } from '../../../app/core/component.js'
import router from '../../../app/core/router.js'
import HeaderTop from '../../../app/layouts/client/top-header/top-header.js'
import LivesPage from './lives-page.js'
import NewsPage from './news-page.js'
import TrendsPage from './trends-page.js'
import TVAppPage from './tv-app-page.js'

// const HomePage = customElement(
//   'home',
//   class extends Component {
//     constructor () {
//       super()

//       this.routes = {
//         news: NewsPage,
//         app: TVAppPage,
//         live: LivesPage
//       }
//     }

//     render () {
//       const { children } = this.props
//       const querySearch = router.getQueries()
//       const tab = querySearch.tab
//       console.log(tab)

//       return `
//       <div class="page-content-container">
//         <${HeaderTop}></${HeaderTop}>
//         <div class="page-content-body">
//           ${
//             tab
//               ? `<${this.routes[tab]}></${this.routes[tab]}>`
//               : `<${TrendsPage}>></${TrendsPage}>`
//           }
//         </div>
//       </div>`
//     }
//   }
// )

// export default HomePage

const HomePage = customElement(
  'home',
  class extends Component {
    constructor () {
      super()

      // this.routes = {
      //   news: NewsPage,
      //   app: TVAppPage,
      //   live: LivesPage
      // }

      // this.handleTabChange = this.handleTabChange.bind(this)
    }

    // connectedCallback () {
    //   super.connectedCallback()
    //   // router.onChange(this.handleTabChange) // Listen for changes in the query parameters
    //   this.handleTabChange()
    //   window.addEventListener('popstate', this.handleTabChange)
    // }

    // disconnectedCallback () {
    //   super.disconnectedCallback()
    //   // router.offChange(this.handleTabChange) // Stop listening for changes in the query parameters
    //   window.removeEventListener('popstate', this.handleTabChange)
    // }

    // handleTabChange () {
    //   console.log('helloworld')
    //   const querySearch = router.getQueries()
    //   const tab = querySearch.tab

    //   // Re-render the content in the page-content-body based on the new tab value
    //   const pageContentBody = this.$('.page-content-body')
    //   if (pageContentBody) {
    //     pageContentBody.innerHTML = tab
    //       ? `<${this.routes[tab]}></${this.routes[tab]}>`
    //       : `<${TrendsPage}></${TrendsPage}>`
    //   }
    // }

    render () {
      const { children } = this.props
      console.log(children)
      return `
      <div class="page-content-container">
        <${HeaderTop}></${HeaderTop}>
        <div id="page-content-body">
        ${children} 
        </div>
      </div>`
    }
  }
)

export default HomePage
