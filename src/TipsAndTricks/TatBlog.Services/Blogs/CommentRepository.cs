using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
  public class CommentRepository : ICommentRepository
  {
    private readonly BlogDbContext _context;
    public CommentRepository(BlogDbContext context)
    {
      _context = context;
    }


    public async Task<int> CountCommentApprovedByIdPostAsync(int postId, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .CountAsync(c => c.PostId == postId & c.IsApproved ?? false, cancellationToken);
    }

    public async Task<Comment> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
    {
      if (comment.Id > 0)
      {
        _context.Set<Comment>().Update(comment);
      }
      else
      {
        _context.Set<Comment>().Add(comment);
      }

      await _context.SaveChangesAsync(cancellationToken);
      return comment;
    }

    public async Task<bool> DeleteCommentAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.Id == id)
        .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<IList<Comment>> GetAllCommentsIsApprovedByIdPostAsync(int postId, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.PostId == postId & c.IsApproved ?? false)
        .ToListAsync(cancellationToken);
    }

    public async Task<Comment> GetCommentByIdAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>().FindAsync(id, cancellationToken);
    }

    public async Task<IList<Comment>> GetCommentsByEmailAsync(
      string email,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.Email == email)
        .ToListAsync(cancellationToken);
    }

    public async Task<IList<Comment>> GetCommentsByPostIdAsync(
      int postId,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.PostId == postId)
        .ToListAsync(cancellationToken);
    }

    private IQueryable<Comment> FilterComment(
     CommentQuery condition)
    {
      return _context.Set<Comment>()
        .Include(c => c.Post)
        .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), c =>
            c.Content.Contains(condition.Keyword)
            || c.NameUserComment.Contains(condition.Keyword)
            || c.Feedback.Contains(condition.Keyword)
            || c.Email.Contains(condition.Keyword))
        .WhereIf(condition.CommentedYear > 0, c =>
            c.CommentedDate.Year == condition.CommentedYear)
        .WhereIf(condition.CommentedMonth > 0, c =>
            c.CommentedDate.Month == condition.CommentedMonth)
        .WhereIf(condition.NotApprovedOnly, c =>
            c.IsApproved == false);
    }

    public Task<IPagedList<T>> GetPagedCommentsAsync<T>(
      CommentQuery query,
      int pageNumber,
      int pageSize,
      Func<IQueryable<Comment>, IQueryable<T>> mapper,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Comment> commentsQuery = FilterComment(query);

      IQueryable<T> commentsQueryResult = mapper(commentsQuery);

      return commentsQueryResult.ToPagedListAsync<T>(
        pageNumber,
        pageSize,
        sortColumn,
        sortOrder);
    }


    public async Task<bool> AprroveCommentAsync(
      int id,
      bool isApprove = true,
      CancellationToken cancellationToken = default)
    {
      Comment comment = await GetCommentByIdAsync(id, cancellationToken);
      if (comment == null)
      {
        return false;
      }

      comment.IsApproved = isApprove;
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }
}
