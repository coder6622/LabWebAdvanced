import config from 'config';
import About from '../pages/client/About';
import Contact from '../pages/client/Contact';
import { default as HomeClient } from 'pages/client/Home';
import { default as HomeAdmin } from 'pages/admin/Home';
import Post from '../pages/client/Post';
import PostsByAuthor from '../pages/client/PostsByAuthor';
import PostsByCategory from '../pages/client/PostsByCategory';
import Rss from '../pages/client/Rss';
import AdminLayout from 'layouts/AdminLayout/AdminLayout';
import Categories from 'pages/admin/Categories';
import Authors from 'pages/admin/Authors';
import Tags from 'pages/admin/Tags';
import Posts from 'pages/admin/Post/Posts';
import Comments from 'pages/admin/Comments';
import NotFound from 'pages/shared/NotFound';
import BadRequest from 'pages/shared/BadRequest';
import OnlyHeaderLayout from 'layouts/OnlyHeaderLayout/OnlyHeaderLayout';

const publicRoutes = [
  { path: config.publicRoutes.home, component: HomeClient },
  { path: config.publicRoutes.blog, component: HomeClient },
  { path: config.publicRoutes.contact, component: Contact },
  { path: config.publicRoutes.about, component: About },
  { path: config.publicRoutes.rss, component: Rss },
  { path: config.publicRoutes.post + '/:slug', component: Post },
  { path: config.publicRoutes.author + '/:AuthorId', component: PostsByAuthor },
  { path: config.publicRoutes.category + '/:CategoryId', component: PostsByCategory },
  { path: config.publicRoutes.notFound, component: NotFound, layout: OnlyHeaderLayout },
  { path: config.publicRoutes.badRequest, component: BadRequest, layout: OnlyHeaderLayout },
];

const privateRoutes = [
  { path: config.privateRoutes.home, component: HomeAdmin, layout: AdminLayout },
  { path: config.privateRoutes.categories, component: Categories, layout: AdminLayout },
  { path: config.privateRoutes.authors, component: Authors, layout: AdminLayout },
  { path: config.privateRoutes.tags, component: Tags, layout: AdminLayout },
  { path: config.privateRoutes.posts, component: Posts, layout: AdminLayout },
  { path: config.privateRoutes.comments, component: Comments, layout: AdminLayout },
];

export { publicRoutes, privateRoutes };
