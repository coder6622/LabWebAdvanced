using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
  public class BlogRepository : IBlogRepository
  {
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<Post> GetPostsAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
    {
      IQueryable<Post> postsQuery = _context.Set<Post>()
        .Include(p => p.Category)
        .Include(p => p.Author);

      if (year > 0)
      {
        postsQuery = postsQuery.Where(p => p.PostedDate.Year == year);
      }

      if (month > 0)
      {
        postsQuery = postsQuery.Where(p => p.PostedDate.Month == month);
      }

      if (!string.IsNullOrEmpty(slug))
      {
        postsQuery = postsQuery.Where(p => p.UrlSlug == slug);
      }

      return await postsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Post>()
        .Include(p => p.Author)
        .Include(p => p.Category)
        .OrderByDescending(p => p.ViewCount)
        .Take(numPosts)
        .ToListAsync(cancellationToken);
    }

    public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
    {
      await _context.Set<Post>()
        .Where(p => p.Id == postId)
        .ExecuteUpdateAsync(p =>
          p.SetProperty(p => p.ViewCount, p => p.ViewCount + 1),
          cancellationToken
        );

    }

    public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Post>()
        .AnyAsync(p => p.Id != postId && p.UrlSlug == slug, cancellationToken);
    }

    public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
    {
      IQueryable<Category> categories = _context.Set<Category>();

      if (showOnMenu)
      {
        categories = categories.Where(c => c.ShowOnMenu);
      }

      return await categories
        .OrderBy(c => c.Name)
        .Select(c => new CategoryItem()
        {
          Id = c.Id,
          Name = c.Name,
          UrlSlug = c.UrlSlug,
          Description = c.Description,
          ShowOnMenu = c.ShowOnMenu,
          PostCount = c.Posts.Count(p => p.Published)
        })
        .ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default)
    {
      IQueryable<TagItem> tagQuery = _context.Set<Tag>()
        .Select(t => new TagItem()
        {
          Id = t.Id,
          Name = t.Name,
          UrlSlug = t.UrlSlug,
          Description = t.Description,
          PostCount = t.Posts.Count(p => p.Published)
        });

      return await tagQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IList<AmountPostItem>> CountPostsInNMonthsAsync(int month, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Post>()
        .Select(p => new AmountPostItem()
        {
          Year = p.PostedDate.Year,
          Month = p.PostedDate.Month,
          PostCount = _context.Set<Post>()
            .Where(x => x.PostedDate == p.PostedDate)
            .Count()
        })
        .Distinct()
        .OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
        .Take(3)
        .ToListAsync(cancellationToken);
    }

    public async Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Post>()
        .Where(p => p.Id == id)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default)
    {

      if (IsPostSlugExistedAsync(post.Id, post.UrlSlug).Result)
      {
        await Console.Out.WriteLineAsync("Url slug exists, input id to edit");
      }
      else
      {
        if (post.Id > 0)
        {
          Post postEditted = await _context.Set<Post>()
            .Include(p => p.Tags)
            .Where(p => p.Id == post.Id)
            .FirstOrDefaultAsync(cancellationToken);

          foreach (var tag in post.Tags.Except(postEditted.Tags).ToList())
          {
            postEditted.Tags.Add(tag);
          }

          foreach (var tag in postEditted.Tags.Except(post.Tags).ToList())
          {
            postEditted.Tags.Remove(tag);
          }

          //postEditted.Tags = post.Tags;

          _context.Entry(postEditted).CurrentValues.SetValues(post);
        }
        else
        {
          _context.Set<Post>().Add(post);
        }
        await _context.SaveChangesAsync(cancellationToken);
      }
    }

    public async Task ChangePostPusblishedStateAsync(int id, bool isPublished, CancellationToken cancellationToken = default)
    {
      await _context.Set<Post>()
       .Where(p => p.Id == id)
       .ExecuteUpdateAsync(p =>
         p.SetProperty(p => p.Published, isPublished), cancellationToken
       );
      ;
    }

    public async Task<IList<Post>> GetRandomNPosts(int n, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Post>()
        .OrderBy(p => p.Id)
        .Take(n)
        .ToListAsync(cancellationToken);
    }
  }
}
