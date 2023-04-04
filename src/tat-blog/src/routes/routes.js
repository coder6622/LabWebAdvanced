import config from '../config';
import About from '../pages/About';
import Contact from '../pages/Contact';
import Home from '../pages/Home';
import Rss from '../pages/Rss';

const publicRoutes = [
  { path: config.routes.home, component: Home },
  { path: config.routes.blog, component: Home },
  { path: config.routes.contact, component: Contact },
  { path: config.routes.about, component: About },
  { path: config.routes.rss, component: Rss },
];

const privateRoutes = [];

export { publicRoutes, privateRoutes };
