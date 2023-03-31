using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
  public class StatisticalRepository : IStatisticalRepository
  {
    private readonly BlogDbContext _context;
    public StatisticalRepository(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<StatisticalItem> GetStatisticals(CancellationToken cancellationToken = default)
    {
      return new StatisticalItem()
      {
        PostCount = await GetPostCount(),
        PostCountNotPushlished = await GetPostCount(true),
        CategoryCount = await GetCategoriesCount(),
        AuthorCount = await GetAuthorCount(),
        CommentCount = await GetCommentCount()
      };
    }

    public async Task<int> GetPostCount(
      bool notPublishedOnly = false,
      CancellationToken cancellationToken = default)
    {
      if (notPublishedOnly)
      {
        return await _context.Set<Post>()
          .Where(p => !p.Published)
          .CountAsync(cancellationToken);
      }

      return await _context.Set<Post>()
        .CountAsync(cancellationToken);
    }

    public async Task<int> GetCategoriesCount(CancellationToken cancellationToken = default)
    {
      return await _context.Set<Category>()
        .CountAsync(cancellationToken);
    }

    public async Task<int> GetAuthorCount(CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .CountAsync(cancellationToken);
    }

    public async Task<int> GetCommentCount(
      bool notApprovedOnly = false,
      CancellationToken cancellationToken = default)
    {
      if (notApprovedOnly)
      {
        return await _context.Set<Comment>()
            .CountAsync(cancellationToken);
      }

      return await _context.Set<Comment>()
        .Where(c => c.IsApproved == false)
        .CountAsync(cancellationToken);
    }

  }
}
