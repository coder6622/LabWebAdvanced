import config from '../config';
import About from '../pages/About';
import Contact from '../pages/Contact';
import Home from '../pages/Home';
import Post from '../pages/Post';
import PostsByAuthor from '../pages/PostsByAuthor';
import PostsByCategory from '../pages/PostsByCategory';
import Rss from '../pages/Rss';
const publicRoutes = [
  { path: config.routes.home, component: Home },
  { path: config.routes.blog, component: Home },
  { path: config.routes.contact, component: Contact },
  { path: config.routes.about, component: About },
  { path: config.routes.rss, component: Rss },
  { path: config.routes.post + '/:slug', component: Post },
  { path: config.routes.author + '/:AuthorId', component: PostsByAuthor },
  { path: config.routes.category + '/:CategoryId', component: PostsByCategory },
];

const privateRoutes = [];

export { publicRoutes, privateRoutes };
