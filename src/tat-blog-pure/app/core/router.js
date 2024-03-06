import {
  exactHashPath,
  getCurrentPath,
  getPrevPath,
  getQueries
} from '../utils/utils.js'

class Router {
  constructor () {
    this.init()
  }

  async init () {
    window.addEventListener('click', event => {
      if (event.target.tagName === 'A') {
        event.preventDefault()
        const path = event.target.getAttribute('href')
        if (path !== getCurrentPath()) {
          const checkedPrivatePath = this.checkPrivateRoute(path)
          this.navigate(checkedPrivatePath)
        }
      }
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

  checkPrivateRoute (path) {
    const splitPath = exactHashPath(path.split('?')[0])

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

  navigate (path) {
    window.history.pushState({ prevUrl: window.location.hash }, null, path)
    this.render()
  }

  render () {
    const { currentPath, queries } = getQueries()

    const tab = queries.tab

    const route = this.routes[currentPath]

    const viewParent = route.view
    const viewListChild = route.child
    const viewChild = viewListChild
      ? tab
        ? this.routes[currentPath].child[tab]
        : Object.values(this.routes[currentPath].child)[0]
      : null

    if (!viewParent) {
      this.navigate('/#/error')
    } else if (this.parentComponent) {
      if (this.myApp.innerHTML.trim() == '') {
        if (viewChild) {
          console.log('has viewchild')
          this.myApp.innerHTML = `<${this.parentComponent} 
            childrens="<${viewParent}></${viewParent}>"></${this.parentComponent}>`
          const pageContentBody = document.querySelector(
            `${viewParent} #page-content-body`
          )
          pageContentBody.innerHTML = `<${viewChild}></${viewChild}>`
        } else {
          this.myApp.innerHTML = `<${this.parentComponent} 
          childrens="<${viewParent}></${viewParent}>"/>`
        }
      } else {
        console.log('render')
        if (getPrevPath() !== currentPath) {
          const existingChildElement = document.querySelector(
            'app-main-layout #router-slot'
          )
          existingChildElement.innerHTML = ''
          existingChildElement.innerHTML = `<${viewParent}></${viewParent}>`
          if (viewListChild) {
            const pageContentBody = document.querySelector('#page-content-body')
            pageContentBody.innerHTML = `<${viewChild}></${viewChild}>`
          }
        } else if (viewListChild) {
          console.log('render tab')
          const pageContentBody = document.querySelector('#page-content-body')
          pageContentBody.innerHTML = `<${viewChild}></${viewChild}>`
        }
      }
    } else {
      this.myApp.innerHTML = `<${viewParent}/>`
    }
  }
}

const router = new Router()
export default router
