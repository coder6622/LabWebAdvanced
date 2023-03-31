using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SlugGenerator;
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

namespace TatBlog.Services.Blogs
{
  public class BlogRepository : IBlogRepository
  {
    private readonly BlogDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public BlogRepository(
      BlogDbContext context,
      IMemoryCache memoryCache)
    {
      _context = context;
      _memoryCache = memoryCache;
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
        .Include(p => p.Author)
        .Include(p => p.Tags)
        .Include(p => p.Comments);

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
        .Where(p => p.Published)
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
      bool showOnMenu = true,
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
          PostCount = c.Posts.Count()
          //p => p.Published)

        })
        .ToListAsync(cancellationToken);
    }


    public async Task<Tag> GetTagByIdAsync(
       int id,
       bool isDetail = false,
       CancellationToken cancellationToken = default)
    {
      if (isDetail)
      {
        return await _context.Set<Tag>()
          .Include(t => t.Posts)
          .Where(t => t.Id == id)
          .FirstOrDefaultAsync(cancellationToken);
      }
      return await _context.Set<Tag>()
        .FindAsync(id, cancellationToken);
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
    public IQueryable<Tag> FilterTags(
      TagQuery condition)
    {
      return _context.Set<Tag>()
        .Include(t => t.Posts)
        .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), t =>
            t.Name.Contains(condition.Keyword)
            || t.Description.Contains(condition.Keyword)
            || t.UrlSlug.Contains(condition.Keyword));
    }

    public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
        TagQuery query,
        int pageNumber,
        int pageSize,
        Func<IQueryable<Tag>, IQueryable<T>> mapper,
        string sortColumn = "Id",
        string sortOrder = "ASC",
        CancellationToken cancellationToken = default)
    {
      IQueryable<Tag> tagsQuery = FilterTags(query);

      IQueryable<T> resultQuery = mapper(tagsQuery);

      return await resultQuery.ToPagedListAsync<T>(
        pageNumber,
        pageSize,
        sortColumn,
        sortOrder,
        cancellationToken);
    }


    public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
             TagQuery query,
             IPagingParams pagingParams,
             Func<IQueryable<Tag>, IQueryable<T>> mapper,
             CancellationToken cancellationToken = default)
    {
      IQueryable<Tag> tagQueryable = FilterTags(query);

      return await mapper(tagQueryable)
        .ToPagedListAsync(pagingParams);
    }


    public async Task<bool> IsTagSlugExistAsync(
       int id,
       string slug,
       CancellationToken cancellationToken = default)
    {
      return await _context.Set<Tag>()
        .AnyAsync(t => t.Id != id && t.UrlSlug == slug);
    }

    public async Task<bool> AddOrUpdateCategoryAsync(
      Category category,
      CancellationToken cancellationToken = default
    )
    {
      if (category.Id > 0)
      {
        _context.Set<Category>().Update(category);
      }
      else
      {
        _context.Set<Category>().Add(category);
      }

      return await _context.SaveChangesAsync(cancellationToken) > 0;
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

    public async Task<Category> GetCategoryByIdAsync(
      int id,
      bool isDetail = false,
      CancellationToken cancellationToken = default
    )
    {
      if (isDetail)
      {
        return await _context
          .Set<Category>()
          .Include(c => c.Posts)
          .Where(c => c.Id == id)
          .FirstOrDefaultAsync(cancellationToken);
      }

      return await _context
        .Set<Category>()
        .FindAsync(id, cancellationToken);
    }

    public Task<Category> GetCachedCategoryByIdAsync(
            int id,
            bool includeDetail = false,
            CancellationToken cancellationToken = default)
    {
      return _memoryCache.GetOrCreateAsync(
        $"category.by-id.{id})",
        async (entry) =>
        {
          entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
          return await GetCategoryByIdAsync(id, includeDetail, cancellationToken);
        });
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

    public async Task<bool> IsCategorySlugExistAsync(
       int id,
       string slug,
       CancellationToken cancellationToken = default)
    {
      return await _context.Set<Category>()
        .AnyAsync(c => c.Id != id
          && c.UrlSlug == slug,
          cancellationToken);
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
      bool includeDetail = false,
      CancellationToken cancellationToken = default
    )
    {

      if (includeDetail)
      {
        return await _context.Set<Tag>()
          .Include(t => t.Posts)
          .Where(t => t.UrlSlug == slug)
          .FirstOrDefaultAsync(cancellationToken);
      }

      return await _context.Set<Tag>()
        .Where(t => t.UrlSlug == slug)
        .FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<bool> AddOrUpdateTagAsync(
       Tag tag,
       CancellationToken cancellationToken = default)
    {
      if (tag.Id > 0)
      {
        _context.Set<Tag>().Update(tag);
      }
      else
      {
        _context.Set<Tag>().Add(tag);
      }

      return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveTagByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    )
    {
      //c1: find->delete
      var tagToDelete = await GetTagByIdAsync(id, true, cancellationToken);
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

    public async Task<IList<AmountPostItemByMonth>> CountPostsInNMonthsAsync(
      int n,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Post>()
        .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
        .Select(p => new AmountPostItemByMonth()
        {
          Year = p.Key.Year,
          Month = p.Key.Month,
          PostCount = p.Count()
        })
        .OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
        .Take(n)
        .ToListAsync(cancellationToken);
    }

    public async Task<Post> GetPostByIdAsync(
      int id,
      bool includeDetails = false,
      CancellationToken cancellationToken = default
    )
    {
      if (!includeDetails)
      {
        return await _context.Set<Post>().FindAsync(id);
      }

      return await _context.Set<Post>()
        .Include(p => p.Tags)
        .Include(p => p.Author)
        .Include(p => p.Category)
        .Include(p => p.Comments)
        .Where(p => p.Id == id)
        .FirstOrDefaultAsync(cancellationToken);
    }


    public Task<Post> GetPostBySlugAsync(
        string slug,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
      if (!includeDetails)
      {
        return _context.Set<Post>()
          .Where(p => p.UrlSlug == slug)
          .FirstOrDefaultAsync(cancellationToken);
      }
      return _context.Set<Post>()
        .Include(p => p.Author)
        .Include(p => p.Tags)
        .Include(p => p.Comments)
        .Include(p => p.Category)
        .Where(p => p.UrlSlug == slug)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AddOrUpdatePostAsync(
      Post post,
      IEnumerable<string> tags,
      CancellationToken cancellationToken = default
    )
    {

      if (post.Id > 0)
      {
        await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
      }
      else
      {
        post.Tags = new List<Tag>();
      }

      var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => new
        {
          Name = x,
          Slug = x.GenerateSlug(),
        })
        .GroupBy(x => x.Slug)
        .ToDictionary(g => g.Key, g => g.First().Name);


      foreach (var kv in validTags)
      {
        if (post.Tags.Any(t => string.Compare(t.UrlSlug, kv.Key,
            StringComparison.InvariantCultureIgnoreCase) == 0))
        {
          continue;
        }

        var tag = await GetTagBySlugAsync(kv.Key, false, cancellationToken) ?? new Tag()
        {
          Name = kv.Value,
          Description = kv.Value,
          UrlSlug = kv.Key,
        };


        post.Tags.Add(tag);
      }

      post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

      if (post.Id > 0)
      {
        _context.Update(post);
      }
      else
        _context.Add(post);

      return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task ChangePostPusblishedStateAsync(
      int id,
      CancellationToken cancellationToken = default
    )
    {
      await _context.Set<Post>()
       .Where(p => p.Id == id)
       .ExecuteUpdateAsync(
           p => p.SetProperty(p => p.Published, p => !p.Published),
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

    public async Task<bool> SetImageUrlPostAsync(
           int postId, string imageUrl,
           CancellationToken cancellationToken = default)
    {
      return await _context.Set<Post>()
        .Where(p => p.Id == postId)
        .ExecuteUpdateAsync(p =>
          p.SetProperty(p => p.ImageUrl, p => imageUrl)
           .SetProperty(p => p.ModifiedDate, p => DateTime.Now),
          cancellationToken) > 0;
    }

    public async Task<IList<T>> GetRandomNPosts<T>(
         int n,
         Func<IQueryable<Post>, IQueryable<T>> mapper,
         CancellationToken cancellationToken = default)
    {
      IQueryable<Post> randomPostQuerable = _context.Set<Post>()
        .Include(p => p.Category)
        .Include(p => p.Author)
        .Include(p => p.Comments)
        .Include(p => p.Tags)
        .OrderBy(p => Guid.NewGuid())
        .Take(n);

      return await mapper(randomPostQuerable)
        .ToListAsync(cancellationToken);
    }

    private IQueryable<Post> FilterPostsByQuery(PostQuery query)
    {
      IQueryable<Post> postsQuery = _context.Set<Post>()
        .Include(p => p.Author)
        .Include(p => p.Category)
        .Include(p => p.Tags);

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

      if (query.PostedYear > 0)
      {
        postsQuery = postsQuery
          .Where(p => p.PostedDate.Year == query.PostedYear);
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

      if (!string.IsNullOrWhiteSpace(query.CategorySlug))
      {
        postsQuery = postsQuery
          .Where(p => p.Category.UrlSlug == query.CategorySlug);
      }

      if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
      {
        postsQuery = postsQuery
          .Where(p => p.Author.UrlSlug == query.AuthorSlug);
      }

      if (!string.IsNullOrWhiteSpace(query.TagSlug))
      {
        postsQuery = postsQuery
          .Where(p => p.Tags.Any(t => t.UrlSlug == query.TagSlug));
      }

      if (query.PublishedOnly)
      {
        postsQuery = postsQuery.Where(p => p.Published);
      }

      if (query.NotPublished)
      {
        postsQuery = postsQuery.Where(p => !p.Published);
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

    public async Task<IList<Post>> GetPostsByQueryAsync(
      PostQuery query,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Post> postsFindResultQuery = FilterPostsByQuery(query);
      return await postsFindResultQuery.ToListAsync(cancellationToken);
    }

    public async Task<int> CountPostsSatisfyQueryAsync(
      PostQuery query,
      CancellationToken cancellationToken = default
    )
    {
      var postsFindResultQuery = await Task.Run(() => GetPostsByQueryAsync(query));
      return postsFindResultQuery.Count;
    }

    public async Task<IPagedList<Post>> GetPagedPostsAsync(
      PostQuery query,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Post> postsFindResultQuery = FilterPostsByQuery(query);
      return await postsFindResultQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
      PostQuery query,
      IPagingParams pagingParams,
      Func<IQueryable<Post>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default)
    {
      IQueryable<Post> postsFindResultQuery = FilterPostsByQuery(query);
      IQueryable<T> tResultQuery = mapper(postsFindResultQuery);

      return await tResultQuery
        .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<Post>> GetPagedPostsAsync(
      PostQuery query,
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
      IQueryable<Post> postsFindResultQuery = FilterPostsByQuery(query);
      await Console.Out.WriteLineAsync(postsFindResultQuery.ToString());
      return await postsFindResultQuery
   .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default)
    {

      var postToDelete = await _context.Set<Post>()
     .Include(p => p.Tags)
     .Include(p => p.Comments)
     .Where(p => p.Id == id)
     .FirstOrDefaultAsync(cancellationToken);
      if (postToDelete == null)
      {
        return false;
      }

      _context.Remove(postToDelete);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }

    public async Task<IList<T>> GetNPostsTopCountAsync<T>(
      int n,
      Func<IQueryable<Post>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default)
    {
      var topPosts = _context.Set<Post>()
        .Include(p => p.Author)
        .Include(p => p.Tags)
        .Include(p => p.Category)
        .Include(p => p.Comments)
        .Where(p => p.Published)
        .OrderByDescending(p => p.ViewCount)
        .Take(n);

      return await mapper(topPosts).ToListAsync(cancellationToken);
    }



    public IQueryable<Category> FilterCategories(
      CategoryQuery condition)
    {

      return _context.Set<Category>()
        .Include(c => c.Posts)
        .WhereIf(!string.IsNullOrWhiteSpace(condition.KeyWord),
          c => c.Name.Contains(condition.KeyWord)
            || c.UrlSlug.Contains(condition.KeyWord)
            || c.Description.Contains(condition.KeyWord))
        .WhereIf(condition.NotShowOnMenu,
          c => !c.ShowOnMenu);
    }

    public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
      CategoryQuery query,
      int pageNumber,
      int pageSize,
      Func<IQueryable<Category>, IQueryable<T>> mapper,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default)
    {
      IQueryable<Category> categoryFilter = FilterCategories(query);

      IQueryable<T> resultQuery = mapper(categoryFilter);

      return await resultQuery
        .ToPagedListAsync<T>(pageNumber, pageSize, sortColumn, sortOrder, cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
        CategoryQuery query,
        IPagingParams pagingParams,
        Func<IQueryable<Category>, IQueryable<T>> mapper,
        CancellationToken cancellationToken = default)
    {
      IQueryable<Category> categoryFilter = FilterCategories(query);

      return await mapper(categoryFilter)
        .ToPagedListAsync<T>(pagingParams, cancellationToken);
    }


    public async Task ChangeCategoriesShowOnMenu(int id, CancellationToken cancellationToken = default)
    {
      await _context.Set<Category>()
       .Where(c => c.Id == id)
       .ExecuteUpdateAsync(
           c => c.SetProperty(c => c.ShowOnMenu, c => !c.ShowOnMenu),
           cancellationToken
       );
      ;
    }
  }
}
