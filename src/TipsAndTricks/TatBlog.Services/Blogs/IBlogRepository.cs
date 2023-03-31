using Microsoft.EntityFrameworkCore;
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
      bool showOnMenu = true,
      CancellationToken cancellationToken = default);
    Task<IList<Post>> GetPopularArticlesAsync(
      int numPosts,
      CancellationToken cancellationToken = default);

    Task<bool> IsPostSlugExistedAsync(
      int postId,
      string slug,
      CancellationToken cancellationToken = default);

    Task<bool> IsPostIdExistedAsync(
      int postId,
      CancellationToken cancellationToken = default);

    Task ChangeCategoriesShowOnMenu(
    int id,
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

    Task<Category> GetCategoryByIdAsync(
      int id,
      bool isDetail = false,
      CancellationToken cancellationToken = default);

    Task<Category> GetCachedCategoryByIdAsync(
      int id,
      bool includeDetail = false,
      CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateCategoryAsync(
      Category category,
      CancellationToken cancellationToken = default);

    Task<bool> DeleteCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<bool> IsCategoryExistBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

    Task<bool> IsCategorySlugExistAsync(
      int id,
      string slug,
      CancellationToken cancellationToken = default);


    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default);


    Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
      CategoryQuery query,
      int pageNumber,
      int pageSize,
      Func<IQueryable<Category>, IQueryable<T>> mapper,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
        CategoryQuery query,
        IPagingParams pagingParams,
        Func<IQueryable<Category>, IQueryable<T>> mapper,
        CancellationToken cancellationToken = default);



    // Tag
    Task<Tag> GetTagByIdAsync(
      int id,
      bool isDetail = false,
      CancellationToken cancellationToken = default);

    Task<Tag> GetTagBySlugAsync(
      string slug,
      bool isDetail = true,
      CancellationToken cancellationToken = default);

    Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedTagsAsync<T>(
        TagQuery query,
        int pageNumber,
        int pageSize,
        Func<IQueryable<Tag>, IQueryable<T>> mapper,
        string sortColumn = "Id",
        string sortOrder = "ASC",
        CancellationToken cancellationToken = default);


    Task<IPagedList<T>> GetPagedTagsAsync<T>(
         TagQuery query,
         IPagingParams pagingParams,
         Func<IQueryable<Tag>, IQueryable<T>> mapper,
         CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateTagAsync(
      Tag category,
      CancellationToken cancellationToken = default);

    Task<bool> RemoveTagByIdAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<bool> IsTagSlugExistAsync(
      int id,
      string slug,
      CancellationToken cancellationToken = default);

    //Post
    Task<IList<AmountPostItemByMonth>> CountPostsInNMonthsAsync(
      int month,
      CancellationToken cancellationToken = default);

    Task<Post> GetPostByIdAsync(
      int id,
      bool includeDetails = false,
      CancellationToken cancellationToken = default);

    Task<Post> GetPostBySlugAsync(
      string slug,
      bool includeDetails = false,
      CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdatePostAsync(
      Post post,
      IEnumerable<string> tags,
      CancellationToken cancellationToken = default);

    Task ChangePostPusblishedStateAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<IList<Post>> GetRandomNPosts(
      int n,
      CancellationToken cancellationToken = default);

    Task<IList<T>> GetRandomNPosts<T>(
        int n,
        Func<IQueryable<Post>, IQueryable<T>> mapper,
        CancellationToken cancellationToken = default);


    Task<bool> SetImageUrlPostAsync(
          int postId, string imageUrl,
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

    Task<bool> DeletePostByIdAsync(
       int id,
       CancellationToken cancellationToken = default);

    Task<IList<T>> GetNPostsTopCountAsync<T>(
      int n,
      Func<IQueryable<Post>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default);
  }
}
