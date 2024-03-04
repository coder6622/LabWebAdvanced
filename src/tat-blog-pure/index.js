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
import HomePage from './features/client/home/home.js'
import DashboardComponent from './features/admin/dashboard/dashboard.js'
import PostDetailComponent from './features/client/post-detail/index.js'
import ContactComponent from './features/client/contact/contact.js'
import AboutPage from './features/client/about/about.js'
import ErrorPage from './features/client/error/error.js'
import MainLayout from './app/layouts/client/main/main.js'
import MainLayoutAdmin from './app/layouts/admin/main/main.js'

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
      view: HomePage,
      layout: MainLayout
    },
    '/dashboard': {
      view: DashboardComponent,
      layout: MainLayoutAdmin,
      privateRoute: authPrivateRoute
    },
    '/posts/post': {
      layout: MainLayout,
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

// window.addEventListener('load', function () {
//   router.render()
// })
