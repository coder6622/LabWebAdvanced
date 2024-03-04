import { exactHashPath } from '../utils/utils.js'

class Router {
  constructor () {
    this.init()
  }

  async init () {
    window.addEventListener('click', event => {
      if (event.target.tagName === 'A') {
        event.preventDefault()
        const path = event.target.getAttribute('href')
        console.log(path, 'to', this.currentPath())
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
    this.parentComponent = elements.parentComponent
    this.myApp = document.querySelector(elements.myApp)
    this.render()
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
    const splitPath = exactHashPath(path.split('?')[0])

    console.log(splitPath)
    const privateRoute = this.routes[splitPath]?.privateRoute
    if (!privateRoute) {
      return path
    }
    const { condition, redirect, message } = privateRoute
    if (condition()) {
      return path
    } else {
      return redirect
    }
  }

  currentPath () {
    const hashPath = window.location.hash
    if (hashPath) {
      return hashPath
    }
    return '/'
  }

  navigate (path) {
    window.history.pushState(null, null, path)
    // this.render()
    window.dispatchEvent(new Event('popstate'))
  }

  render () {
    const currentPath = exactHashPath(this.currentPath())
    const view = this.routes[currentPath]?.view

    if (!view) {
      this.navigate('/#/error')
    } else if (this.parentComponent) {
      this.myApp.innerHTML = `<${this.parentComponent} childrens="<${view}></${view}>"/>`
    } else {
      this.myApp.innerHTML = `<${view}/>`
    }
  }
}

const router = new Router()
export default router
