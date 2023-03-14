using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
  public interface IBlogRepository
  {
    Task<Post> GetPostsAsync(
      int year,
      int month,
      string slug,
      CancellationToken cancellationToken = default);
    Task<IList<CategoryItem>> GetCategoriesAsync(
      bool showOnMenu = false,
      CancellationToken cancellationToken = default);
    Task<IList<Post>> GetPopularArticlesAsync(
      int numPosts,
      CancellationToken cancellationToken = default);
    Task<bool> IsPostSlugExistedAsync(
      int postId,
      string slug,
      CancellationToken cancellationToken = default);
    Task IncreaseViewCountAsync(
      int postId,
      CancellationToken cancellationToken = default);
    Task<IPagedList<TagItem>> GetPagedTagsAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default);

    // ==== Section C ====

    // Category
    Task<Category> FindCategoryBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

    Task<Category> FindCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task AddOrUpdateCategoryAsync(
      Category category,
      CancellationToken cancellationToken = default);

    Task<bool> DeleteCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<bool> IsCategoryExistBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default);

    // Tag
    Task<Tag> GetTagBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

    Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default);

    Task<bool> RemoveTagByIdAsync(
      int id,
      CancellationToken cancellationToken = default);

    //Post
    Task<IList<AmountPostItemByMonth>> CountPostsInNMonthsAsync(
      int month,
      CancellationToken cancellationToken = default);

    Task<Post> FindPostByIdAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task AddOrUpdatePostAsync(
      Post post,
      IList<Tag> tags,
      CancellationToken cancellationToken = default);

    Task ChangePostPusblishedStateAsync(
      int id,
      bool pusblished,
      CancellationToken cancellationToken = default);

    Task<IList<Post>> GetRandomNPosts(
      int n,
      CancellationToken cancellationToken = default);

    Task<IList<Post>> GetPostsByQueryAsync(
      PostQuery query,
      CancellationToken cancellationToken = default);

    Task<int> CountPostsSatisfyQueryAsync(
      PostQuery query,
      CancellationToken cancellationToken = default);

    Task<IPagedList<Post>> GetPagedPostsAsync(
      PostQuery query,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default
    );


    Task<IPagedList<Post>> GetPagedPostsAsync(
      PostQuery query,
      int pageNumber,
      int pageSize,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default
    );

    Task<IPagedList<T>> GetPagedPostsAsync<T>(
      PostQuery query,
      IPagingParams pagingParams,
      Func<IQueryable<Post>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default
    );
  }
}
