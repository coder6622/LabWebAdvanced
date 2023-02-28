﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
  public interface IBlogRepository
  {

    Task<Post> GetPostsAsync(int year, int month, string slug, CancellationToken cancellationToken = default);
    Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);
    Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);
    Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);
    Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);
  }
}