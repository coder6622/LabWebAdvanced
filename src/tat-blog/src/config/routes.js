const publicRoutes = {
  home: '/',
  blog: '/blog',
  contact: '/blog/contact',
  about: '/blog/about',
  rss: '/blog/RSS',
  tag: '/blog/tag',
  post: '/blog/post',
  author: '/author',
  category: '/category',
  notFound: '*',
  badRequest: '/400',
};

const privateRoutes = {
  home: '/admin',
  categories: '/admin/categories',
  authors: '/admin/authors',
  tags: '/admin/tags',
  posts: '/admin/posts',
  comments: '/admin/comments',
};

export { publicRoutes, privateRoutes };
