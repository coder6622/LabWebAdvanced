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

namespace TatBlog.Services.Categories
{
  public class CategoriesRepository : ICategoriesRepository
  {

    private readonly BlogDbContext _context;

    public CategoriesRepository(BlogDbContext context)
    {
      _context = context;
    }

    public async Task AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
      if (await IsCategoryExistBySlugAsync(category.Id, category.UrlSlug))
      {
        await Console.Out.WriteLineAsync("Url slug exists, input id to edit");
      }
      else
      {
        if (category.Id > 0)
        {
          Category categoryEdited = await _context.Set<Category>()
            .Where(c => c.UrlSlug == category.UrlSlug)
            .FirstOrDefaultAsync(cancellationToken);

          _context.Entry(categoryEdited).CurrentValues.SetValues(category);
        }
        else
        {
          _context.Set<Category>().Add(category);
        }
      }
      await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
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

    public async Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
      return await _context
        .Set<Category>()
        .Where(c => c.Id == id)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
      IQueryable<Category> categoriesQuery = _context.Set<Category>();

      if (!string.IsNullOrEmpty(slug))
      {
        categoriesQuery = categoriesQuery.Where(c => c.UrlSlug == slug);
      }

      return await categoriesQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
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

      return await categoriesQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public Task<bool> IsCategoryExistBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
      return _context.Set<Category>()
        .AnyAsync(c => c.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> IsCategoryExistBySlugAsync(int id, string slug, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Category>()
        .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
    }


  }
}
