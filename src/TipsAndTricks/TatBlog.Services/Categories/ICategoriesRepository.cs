using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Categories
{
  public interface ICategoriesRepository
  {
    Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

    Task AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);

    Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> IsCategoryExistBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default);

  }
}
