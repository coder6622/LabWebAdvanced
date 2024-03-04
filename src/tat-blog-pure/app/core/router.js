// import Utils from '../utils/utils.js'
// class Router2 {
//   constructor () {
//     this.routes = {
//       '/': import('../../../features/client/home/index.js').then(
//         m => m.HomeComponent
//       ),
//       '/dashboard': import('../../features/admin/dashboard/dashboard.js').then(
//         m => m.DashboardComponent
//       ),
//       '/post/:id': import('../../features/client/post-detail/index.js').then(
//         m => m.PostDetailComponent
//       ),
//       '/contact': import('../../features/client/contact/contact.js').then(
//         m => m.ContactComponent
//       ),
//       '/about': import('../../features/client/about/about.js').then(
//         m => m.AboutPage
//       )
//     }

//     this.content = document.getElementById('router-slot')
//     this.init()
//   }

//   async init () {
//     const request = new Utils().parseRequestURL()
//     const parsedURL =
//       (request.resource ? '/' + request.resource : '/') +
//       (request.id ? '/:id' : '') +
//       (request.verb ? '/' + request.verb : '')

//     if (this.routes[parsedURL]) {
//       const component = await this.routes[parsedURL]
//       this.content.innerHTML = ''
//       const instance = request.id
//         ? component.create(request)
//         : component.create()

//       this.content.append(instance)
//     }
//   }
// }

// export default Router2

class Router {
  constructor () {
    this.init()
  }

  currentPath () {
    return window.location.pathname
  }

  async init () {
    window.addEventListener('click', event => {
      if (event.target.tagName === 'A') {
        event.preventDefault()
        const path = event.target.getAttribute('href')
        console.log(path, this.currentPath())
        if (path !== this.currentPath()) {
          const checkedPrivatePath = this.checkPrivateRoute(path)
          this.navigate(checkedPrivatePath)
        }
      }
    })

    window.addEventListener('hashchange', () => {
      this.render()
    })

    window.addEventListener('popstate', () => {
      this.render()
    })
  }

  addRoutes (elements, routes) {
    this.routes = routes

    console.log(elements)
    console.log(elements.parentComponent)
    this.parentComponent = elements.parentComponent

    this.myApp = document.querySelector(elements.myApp)
    this.render()

    console.log(this.parentComponent, this.myApp)
  }

  getQueries () {
    const queries = {}
    const search = window.location.search

    if (search) {
      const queryStr = search.slice(1) // remove ?
      const queryArr = queryStr.split('&')
      queryArr.forEach(item => {
        const [key, value] = item.split('=')
        queries[key] = value
      })
    }
    return queries
  }

  checkPrivateRoute (path) {
    // const splitPath = path.split('?')[0]
    // const { privateRoute } = this.routes[splitPath]
    // if (!privateRoute) {
    //   console.log('public route')
    //   return path
    // }
    // const { condition, redirect, message } = privateRoute

    // if (condition()) {
    //   return path
    // } else {
    //   return redirect
    // }
    return path
  }

  navigate (path) {
    window.history.pushState('', '', path)
    this.render()
  }

  render () {
    const currentPath = this.currentPath()
    const { view } = this.routes[currentPath]

    if (this.parentComponent) {
      this.myApp.innerHTML = `<${this.parentComponent} children="<${view}></${view}>"/>`
    } else if (view) {
      this.myApp.innerHTML = `<${view}/>`
    } else {
      this.navigate('/error')
    }
  }
}

const router = new Router()
export default router
