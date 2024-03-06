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
import MainLayout from './app/layouts/client/main/main.js'
import './app/layouts/client/sidebar/sidebar.js'
import DashboardComponent from './features/admin/dashboard/dashboard.js'
import AboutPage from './features/client/about/about.js'
import ContactComponent from './features/client/contact/contact.js'
import ErrorPage from './features/client/error/error.js'
import HomePage from './features/client/home/home.js'
import LivesPage from './features/client/home/lives-page.js'
import NewsPage from './features/client/home/news-page.js'
import TrendsPage from './features/client/home/trends-page.js'
import TVAppPage from './features/client/home/tv-app-page.js'
import PostDetailComponent from './features/client/post-detail/index.js'

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
      child: {
        '': TrendsPage,
        news: NewsPage,
        app: TVAppPage,
        live: LivesPage
      }
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

// window.addEventListener('load', function () {
//   router.render()
// })
