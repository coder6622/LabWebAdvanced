using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

namespace TatBlog.Services.Blogs
{
  public class BlogRepository : IBlogRepository
  {
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<Post> GetPostsAsync(
      int year,
      int month,
      string slug,
      CancellationToken cancellationToken = default
    )
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

    public async Task<IList<Post>> GetPopularArticlesAsync(
      int numPosts,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Post>()
        .Include(p => p.Author)
        .Include(p => p.Category)
        .OrderByDescending(p => p.ViewCount)
        .Take(numPosts)
        .ToListAsync(cancellationToken);
    }

    public async Task IncreaseViewCountAsync(
      int postId,
      CancellationToken cancellationToken = default
    )
    {
      await _context.Set<Post>()
        .Where(p => p.Id == postId)
        .ExecuteUpdateAsync(p =>
          p.SetProperty(p => p.ViewCount, p => p.ViewCount + 1),
          cancellationToken
        );

    }

    public async Task<bool> IsPostSlugExistedAsync(
      int postId,
      string slug,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Post>()
        .AnyAsync(
          p => p.Id != postId && p.UrlSlug == slug,
          cancellationToken
        );
    }

    public async Task<IList<CategoryItem>> GetCategoriesAsync(
      bool showOnMenu = false,
      CancellationToken cancellationToken = default
    )
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

    public async Task AddOrUpdateCategoryAsync(
      Category category,
      CancellationToken cancellationToken = default
    )
    {
      if (category.Id > 0)
      {
        Category categoryEdited = await _context.Set<Category>()
          .Where(c => c.UrlSlug == category.UrlSlug)
          .FirstOrDefaultAsync(cancellationToken);

        if (categoryEdited.UrlSlug != category.UrlSlug)
        {
          if (IsPostSlugExistedAsync(category.Id, category.UrlSlug).Result)
          {
            await Console.Out.WriteLineAsync("Url slug exists, please change url slug");
            return;
          }
        }

        _context.Entry(categoryEdited).CurrentValues.SetValues(category);
      }
      else
      {
        if (await IsCategoryExistBySlugAsync(category.Id, category.UrlSlug))
        {
          await Console.Out.WriteLineAsync("Url slug exists, please change url slug");
          return;
        }
        _context.Set<Category>().Add(category);
      }
      await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<bool> DeleteCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    )
    {
      var categoryToDelete = await _context
        .Set<Category>()
        .Where(c => c.Id == id)
        .FirstOrDefaultAsync(cancellationToken);

      if (categoryToDelete == null)
      {
        return false;
      }

      _context.Set<Category>().Remove(categoryToDelete);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }

    public async Task<Category> FindCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    )
    {
      return await _context
        .Set<Category>()
        .Where(c => c.Id == id)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Category> FindCategoryBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Category> categoriesQuery = _context.Set<Category>();

      if (!string.IsNullOrEmpty(slug))
      {
        categoriesQuery = categoriesQuery.Where(c => c.UrlSlug == slug);
      }

      return await categoriesQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<CategoryItem> categoriesQuery = _context.Set<Category>()
        .Select(c => new CategoryItem()
        {
          Id = c.Id,
          Description = c.Description,
          Name = c.Name,
          ShowOnMenu = c.ShowOnMenu,
          UrlSlug = c.UrlSlug,
          PostCount = c.Posts.Count(p => p.Published),
        });

      return await categoriesQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public Task<bool> IsCategoryExistBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default
    )
    {
      return _context.Set<Category>()
        .AnyAsync(c => c.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> IsCategoryExistBySlugAsync(
      int id,
      string slug,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Category>()
        .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
    }
    public async Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
      IQueryable<Tag> tags = _context.Set<Tag>();
      return await tags
        .OrderBy(t => t.Name)
        .Select(t => new TagItem()
        {
          Id = t.Id,
          Name = t.Name,
          UrlSlug = t.UrlSlug,
          Description = t.Description,
          PostCount = t.Posts.Count(p => p.Published)
        })
        .ToListAsync(cancellationToken);
      ;
    }

    public async Task<Tag> GetTagBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Tag> tagsQuery = _context.Set<Tag>()
        .Include(t => t.Posts);

      if (!string.IsNullOrEmpty(slug))
      {
        tagsQuery = tagsQuery.Where(t => t.UrlSlug == slug);
      }

      return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> RemoveTagByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    )
    {
      //c1: find->delete
      var tagToDelete = await _context.Set<Tag>()
         .Include(t => t.Posts)
         .Where(t => t.Id == id)
         .FirstOrDefaultAsync(cancellationToken);
      if (tagToDelete == null)
      {
        return false;
      }

      _context.Set<Tag>().Remove(tagToDelete);
      await _context.SaveChangesAsync(cancellationToken);
      return true;


      // c2:  dung cau lenh sql 
      //await _context.Database
      //       .ExecuteSqlRawAsync($"DELETE FROM PostTags WHERE TagsId={id}", cancellationToken);


      // c3: 
      //return await _context.Set<Tag>()
      //           .Where(t => t.Id == id)
      //           .ExecuteDeleteAsync(cancellationToken) > 0;


    }

    public async Task<IList<AmountPostItem>> CountPostsInNMonthsAsync(
      int n,
      CancellationToken cancellationToken = default
    )
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
        .Take(n)
        .ToListAsync(cancellationToken);
    }

    public async Task<Post> FindPostByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Post>()
        .Where(p => p.Id == id)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddOrUpdatePostAsync(
      Post post,
      IList<Tag> tags,
      CancellationToken cancellationToken = default
    )
    {
      if (post.Id > 0)
      {
        Post postEditted = await _context.Set<Post>()
          .Include(p => p.Tags)
          .Where(p => p.Id == post.Id)
          .FirstOrDefaultAsync(cancellationToken);

        if (postEditted.UrlSlug != post.UrlSlug)
        {
          if (IsPostSlugExistedAsync(post.Id, post.UrlSlug).Result)
          {
            await Console.Out.WriteLineAsync("Url slug exists, please change url slug");
            return;
          }
        }

        postEditted.Tags = tags;
        _context.Entry(postEditted).CurrentValues.SetValues(post);
      }
      else
      {
        if (IsPostSlugExistedAsync(post.Id, post.UrlSlug).Result)
        {
          await Console.Out.WriteLineAsync("Url slug exists, please change url slug");
          return;
        }
        post.Tags = tags;
        _context.Set<Post>().Add(post);
      }
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangePostPusblishedStateAsync(
      int id,
      bool isPublished,
      CancellationToken cancellationToken = default
    )
    {
      await _context.Set<Post>()
       .Where(p => p.Id == id)
       .ExecuteUpdateAsync(
           p => p.SetProperty(p => p.Published, isPublished),
           cancellationToken
       );
      ;
    }

    public async Task<IList<Post>> GetRandomNPosts(
      int n,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Post>()
        .OrderBy(p => Guid.NewGuid())
        .Take(n)
        .ToListAsync(cancellationToken);
    }

    private IQueryable<Post> FindPostsByQueryToQueryable(PostQuery query)
    {
      IQueryable<Post> postsQuery = _context.Set<Post>();

      if (!string.IsNullOrEmpty(query.Keyword))
      {
        postsQuery = postsQuery
          .Where(p => p.Title.Contains(query.Keyword)
            || p.Description.Contains(query.Keyword)
            || p.ShortDescription.Contains(query.Keyword)
            || p.UrlSlug.Contains(query.Keyword)
            || p.Tags.Any(t => t.Name.Contains(query.Keyword))
          );
      }

      if (query.PostedMonth > 0)
      {
        postsQuery = postsQuery
          .Where(p => p.PostedDate.Month == query.PostedMonth);
      }

      if (query.CategoryId > 0)
      {
        postsQuery = postsQuery
          .Where(p => p.CategoryId == query.CategoryId);
      }

      if (query.AuthorId > 0)
      {
        postsQuery = postsQuery
          .Where(p => p.AuthorId == query.AuthorId);
      }

      if (!string.IsNullOrEmpty(query.CategoryName))
      {
        postsQuery = postsQuery
            .Where(p => p.Category.Name == query.CategoryName);
      }


      var selectedTags = query.GetSelectedTags();
      if (selectedTags.Count > 0)
      {
        foreach (var tag in selectedTags)
        {
          postsQuery = postsQuery.Include(p => p.Tags)
            .Where(p => p.Tags.Any(t => t.Name == tag));
        }
      }

      return postsQuery;
    }

    public async Task<IList<Post>> FindPostsByQueryAsync(
      PostQuery query,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Post> postsFindResultQuery = FindPostsByQueryToQueryable(query);
      return await postsFindResultQuery.ToListAsync(cancellationToken);
    }

    public async Task<int> CountPostsSatisfyQueryAsync(
      PostQuery query,
      CancellationToken cancellationToken = default
    )
    {
      var postsFindResultQuery = await Task.Run(() => FindPostsByQueryAsync(query));
      return postsFindResultQuery.Count;
    }

    public async Task<IPagedList<Post>> FindAndPaginatePostByQueryAsync(
      PostQuery query,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Post> postsFindResultQuery = FindPostsByQueryToQueryable(query);
      return await postsFindResultQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> FindAndPaginatePostAsync<T>(
      PostQuery query,
      IPagingParams pagingParams,
      Func<IQueryable<Post>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default)
    {
      IQueryable<Post> postsFindResultQuery = FindPostsByQueryToQueryable(query);
      IQueryable<T> tResultQuery = mapper(postsFindResultQuery);

      return await tResultQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }
  }
}
