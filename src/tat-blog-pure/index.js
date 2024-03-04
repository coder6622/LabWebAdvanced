import './app/components/archives/archives.js'
import './app/components/categories/categories.js'
import './app/components/loading/loading.js'
import './app/components/pager/pager.js'
import './app/components/post-iem/post-item.js'
import './app/components/tag/tag-item.js'
import globalState from './app/core/GlobalStates.js'
import router from './app/core/router.js'
import './app/layouts/client/header/header.js'
import './app/layouts/client/main/main.js'
import './app/layouts/client/sidebar/sidebar.js'
import HomeComponent from './features/client/home/index.js'
import DashboardComponent from './features/admin/dashboard/dashboard.js'
import PostDetailComponent from './features/client/post-detail/index.js'
import ContactComponent from './features/client/contact/contact.js'
import AboutPage from './features/client/about/about.js'
import ErrorPage from './features/client/error/error.js'
import MainLayout from './app/layouts/client/main/main.js'

// const isLogin = false

// addEventListener('DOMContentLoaded', function () {
//   const appContainer = document.getElementById('app')
//   if (isLogin) appContainer.append(document.createElement('app-admin-layout'))
//   else appContainer.append(document.createElement('app-main-layout'))
// })
// document.onreadystatechange = function (e) {
//   if (document.readyState === 'complete') {
//   }
// }
// window.addEventListener('load', function () {
//   new Router2()
// })

// window.addEventListener('hashchange', function () {
//   new Router2()
// })

// window.addEventListener('popstate', function () {
//   new Router2()
// })

// function navigateTo (url) {
//   history.pushState(null, null, url)
//   new Router2()
// }

// document.addEventListener('click', function (event) {
//   const target = event.target
//   if (target.tagName === 'A') {
//     event.preventDefault()
//     const href = target.getAttribute('href')
//     if (href) {
//       navigateTo(href)
//     }
//   }
// })

globalState.initStates({
  user: null
})

const authPrivateRoute = {
  condition: () => {
    const { user } = globalState.getStates(this)
    return user
  },
  redirect: '/'
}

router.addRoutes(
  {
    parentComponent: MainLayout,
    myApp: '#app'
  },
  {
    '/': {
      view: HomeComponent
    },
    '/dashboard': {
      view: DashboardComponent,
      privateRoute: authPrivateRoute
    },
    '/posts/post': {
      view: PostDetailComponent
    },
    '/contact': {
      view: ContactComponent
    },
    '/about': {
      view: AboutPage
    },
    '/error': {
      view: ErrorPage
    }
  }
)
