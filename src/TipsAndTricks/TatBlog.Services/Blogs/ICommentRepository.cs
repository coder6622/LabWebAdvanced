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
  public interface ICommentRepository
  {
    Task<Comment> GetCommentByIdAsync(
      int id,
      bool isDetail = false,
      CancellationToken cancellationToken = default);
    Task<IList<Comment>> GetCommentsByPostIdAsync(
      int postId,
      CancellationToken cancellationToken = default);

    Task<IList<Comment>> GetCommentsByEmailAsync(
      string email,
      CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateCommentAsync(
      Comment comment,
      CancellationToken cancellationToken = default);

    Task<bool> DeleteCommentAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<int> CountCommentApprovedByIdPostAsync(
      int postId,
      CancellationToken cancellationToken = default);

    Task<IList<Comment>> GetAllCommentsIsApprovedByIdPostAsync(
      int postId,
      CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedCommentsAsync<T>(
    CommentQuery query,
    int pageNumber,
    int pageSize,
    Func<IQueryable<Comment>, IQueryable<T>> mapper,
    string sortColumn = "Id",
    string sortOrder = "ASC",
    CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedCommentsAsync<T>(
    CommentQuery query,
    IPagingParams pagingParams,
    Func<IQueryable<Comment>, IQueryable<T>> mapper,
    CancellationToken cancellationToken = default);

    Task<bool> AprroveCommentAsync(
      int id,
      bool isApprove = true,
      CancellationToken cancellationToken = default);

  }
}
