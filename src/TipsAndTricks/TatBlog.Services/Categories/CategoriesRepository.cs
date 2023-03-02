using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Categories
{
  public class CategoriesRepository : ICategoriesRepository
  {

    private readonly BlogDbContext _context;

    public CategoriesRepository(BlogDbContext context)
    {
      _context = context;
    }
    public Task<bool> AddOrUpdateCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
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

    public Task<bool> IsCategoryExistBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}
