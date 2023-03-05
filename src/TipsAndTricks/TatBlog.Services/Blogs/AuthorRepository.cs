using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using static Azure.Core.HttpHeader;

namespace TatBlog.Services.Blogs
{
  public class AuthorRepository : IAuthorRepository
  {

    private BlogDbContext _context;
    public AuthorRepository(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<bool> IsAuthorExistBySlugAsync(
      int id,
      string slug,
    CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Author>()
        .AnyAsync(a => a.Id != id && a.UrlSlug == slug, cancellationToken);
    }

    public async Task AddOrUpdateAuthor(Author author, CancellationToken cancellationToken = default)
    {
      if (author.Id > 0)
      {
        Author authorEditted = await Task.Run(() => FindAuthorByIdAsync(author.Id));

        if (authorEditted == null)
        {
          await Console.Out.WriteLineAsync("Author not found");
          return;
        }
        if (authorEditted.UrlSlug != author.UrlSlug
          && await IsAuthorExistBySlugAsync(author.Id, author.UrlSlug))
        {
          await Console.Out.WriteLineAsync("Url slug exists, please change url slug");
          return;
        }

        _context.Entry(authorEditted).CurrentValues.SetValues(author);
      }
      else
      {
        if (await IsAuthorExistBySlugAsync(author.Id, author.UrlSlug))
        {
          await Console.Out.WriteLineAsync("Url slug exists, please change url slug");
          return;
        }
        _context.Set<Author>().Add(author);
      }
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Author> FindAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>().FindAsync(id, cancellationToken);
    }

    public async Task<Author> FindAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .Where(a => a.UrlSlug == slug)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IPagedList<AuthorItem>> GetAllAuthor(IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .Select(a => new AuthorItem()
        {
          Id = a.Id,
          FullName = a.FullName,
          UrlSlug = a.UrlSlug,
          ImageUrl = a.ImageUrl,
          JoinedDate = a.JoinedDate,
          Email = a.Email,
          Notes = a.Notes,
          PostsCount = a.Posts.Count(p => p.Published),
        })
        .ToPagedListAsync(pagingParams, cancellationToken);

    }

    public async Task<IPagedList<Author>> GetNAuthorTopPosts(
      int n,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default
    )
    {
      Author authorsMax = _context.Set<Author>()
        .Include(a => a.Posts)
        .OrderByDescending(a => a.Posts.Count(p => p.Published))
        .First();

      int maxAmmountOfPost = authorsMax.Posts.Count(p => p.Published);

      return await _context.Set<Author>()
        .Include(a => a.Posts)
        .Where(a => a.Posts.Count(p => p.Published) == maxAmmountOfPost)
        .Take(n)
        .ToPagedListAsync(pagingParams, cancellationToken);
    }
  }
}
