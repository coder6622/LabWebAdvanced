using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core;
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

    private readonly BlogDbContext _context;
    private readonly IMemoryCache _memoryCache;
    public AuthorRepository(
      BlogDbContext context,
      IMemoryCache memoryCache)
    {
      _context = context;
      _memoryCache = memoryCache;
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

    public async Task<bool> AddOrUpdateAuthor(
      Author author,
      CancellationToken cancellationToken = default)
    {
      if (author.Id > 0)
      {
        _context.Set<Author>().Update(author);
      }
      else
      {
        _context.Set<Author>().Add(author);
      }

      return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Author> GetAuthorByIdAsync(
      int id,
      bool includeDetail = false,
      CancellationToken cancellationToken = default)
    {
      if (!includeDetail)
      {
        return await _context.Set<Author>()
          .FindAsync(id, cancellationToken);
      }

      return await _context.Set<Author>()
              .Include(a => a.Posts)
              .Where(a => a.Id == id)
              .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Author> GetCachedAuthorByIdAsync(
       int id,
       bool includeDetail = false,
       CancellationToken cancellationToken = default)
    {
      return _memoryCache.GetOrCreateAsync(
        $"author.by-id.{id}",
        async (entry) =>
        {
          entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
          return await GetAuthorByIdAsync(id, includeDetail, cancellationToken);
        });
    }

    public async Task<Author> GetAuthorBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .Where(a => a.UrlSlug == slug)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Author> GetCachedAuthorBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default)
    {
      return _memoryCache.GetOrCreateAsync(
          $"author.by-slug.{slug}",
          async (entry) =>
          {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await GetAuthorBySlugAsync(slug, cancellationToken);
          });
    }

    public async Task<IPagedList<AuthorItem>> GetAllAuthorsAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default)
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
          PostCount = a.Posts.Count(p => p.Published),
        })
        .ToPagedListAsync(pagingParams, cancellationToken);

    }

    public async Task<IPagedList<Author>> GetNAuthorTopPostsAsync(
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

    public async Task<IPagedList<T>> GetNAuthorTopPostsAsync<T>(
       int n,
       IPagingParams pagingParams,
       Func<IQueryable<Author>, IQueryable<T>> mapper,
       CancellationToken cancellationToken = default)
    {
      Author authorsMax = _context.Set<Author>()
        .Include(a => a.Posts)
        .OrderByDescending(a => a.Posts.Count(p => p.Published))
        .First();

      int maxAmmountOfPost = authorsMax.Posts.Count(p => p.Published);

      IQueryable<Author> authors = _context.Set<Author>()
              .Include(a => a.Posts)
              .Where(a => a.Posts.Count > 2
                    && a.Posts.Count <= authorsMax.Posts.Count)
              .Take(n);

      return await mapper(authors).ToPagedListAsync(pagingParams, cancellationToken);
    }


    public async Task<IList<AuthorItem>> GetAllAuthorsAsync(CancellationToken cancellationToken = default)
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
          PostCount = a.Posts.Count(p => p.Published),
        })
        .ToListAsync(cancellationToken);
    }

    private IQueryable<Author> FilterAuthors(
      AuthorQuery condition)
    {
      IQueryable<Author> authQueryable = _context.Set<Author>()
        .AsNoTracking();

      if (!string.IsNullOrWhiteSpace(condition.KeyWord))
      {
        authQueryable = authQueryable
          .Where(a =>
               a.Email.Contains(condition.KeyWord)
            || a.FullName.Contains(condition.KeyWord));

      }

      if (condition.JoinedYear > 0)
      {
        authQueryable = authQueryable
           .Where(a => a.JoinedDate.Year == condition.JoinedYear);
      }

      if (condition.JoinedMonth > 0)
      {
        authQueryable = authQueryable
         .Where(a => a.JoinedDate.Month == condition.JoinedMonth);
      }

      //authQueryable
      //  .WhereIf(!string.IsNullOrWhiteSpace(condition.KeyWord),
      //    a => a.FullName.Contains(condition.KeyWord)
      //    || a.Notes.Contains(condition.KeyWord)
      //    || a.Email.Contains(condition.KeyWord))
      //  .WhereIf(condition.JoinedYear > 0,
      //    a => a.JoinedDate.Year == condition.JoinedYear)
      //  .WhereIf(condition.JoinedMonth > 0,
      //    a => a.JoinedDate.Month == condition.JoinedMonth);

      return authQueryable;

    }

    public Task<IPagedList<Author>> GetPagedAuthorsAsync(
      AuthorQuery authorQuery,
      int pageNumber,
      int pageSize,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default)
    {
      var pagingParams = new PagingParams()
      {
        PageNumber = pageNumber,
        PageSize = pageSize,
        SortColumn = sortColumn,
        SortOrder = sortOrder
      };
      return FilterAuthors(authorQuery)
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
      AuthorQuery query,
      int pageNumber,
      int pageSize,
      Func<IQueryable<Author>, IQueryable<T>> mapper,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default)
    {
      IQueryable<Author> authorFilter = FilterAuthors(query);
      IQueryable<T> tResultQuery = mapper(authorFilter);
      var pagingParams = new PagingParams()
      {
        PageNumber = pageNumber,
        PageSize = pageSize,
        SortColumn = sortColumn,
        SortOrder = sortOrder
      };

      return await tResultQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }


    public async Task<IList<T>> GetAllAuthorsAsync<T>(
          Func<IQueryable<Author>, IQueryable<T>> mapper,
          CancellationToken cancellationToken = default)
    {
      IQueryable<Author> authors = _context.Set<Author>();

      return await mapper(authors).ToListAsync(cancellationToken);
    }


    public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
      IPagingParams pagingParams,
      string name = null,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .AsNoTracking()
        .WhereIf(!string.IsNullOrWhiteSpace(name),
          x => x.FullName.Contains(name))
        .Select(a => new AuthorItem()
        {
          Id = a.Id,
          FullName = a.FullName,
          Email = a.Email,
          JoinedDate = a.JoinedDate,
          ImageUrl = a.ImageUrl,
          UrlSlug = a.UrlSlug,
          PostCount = a.Posts.Count(p => p.Published)
        })
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
      Func<IQueryable<Author>, IQueryable<T>> mapper,
      IPagingParams pagingParams,
      string name = null,
      CancellationToken cancellationToken = default)
    {
      var authorQuery = _context.Set<Author>().AsNoTracking();

      if (!string.IsNullOrEmpty(name))
      {
        authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
      }

      return await mapper(authorQuery)
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> IsAuthorSlugExist(
      int id,
      string slug,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .AnyAsync(a => a.Id != id && a.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
    {
      var author = await _context.Set<Author>().FindAsync(id);
      if (author is null)
        return false;

      _context.Set<Author>().Remove(author);
      var rowsCount = await _context.SaveChangesAsync(cancellationToken);

      return rowsCount > 0;
    }

    public async Task<bool> SetImageUrlAsync(
        int authorId, string imageUrl,
        CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .Where(a => a.Id == authorId)
        .ExecuteUpdateAsync(a =>
          a.SetProperty(a => a.ImageUrl, a => imageUrl),
          cancellationToken) > 0;
    }
  }
}
