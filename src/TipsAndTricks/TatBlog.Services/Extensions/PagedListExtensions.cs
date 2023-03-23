using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;

namespace TatBlog.Services.Extensions
{
  public static class PagedListExtensions
  {
    /// <summary>
    /// Tạo biểu thức sắp xếp dữ liệu sau truy vấn, dùng sau mệnh đề ORDER BY 
    /// </summary>
    /// <param name="pagingParams"></param>
    /// <param name="defaultColumn"></param>
    /// <returns></returns>
    public static string GetOrderExpression(
      this IPagingParams pagingParams,
      string defaultColumn = "Id")
    {
      var column = string.IsNullOrWhiteSpace(pagingParams.SortColumn)
        ? defaultColumn
        : pagingParams.SortColumn;

      var order = "ASC".Equals(pagingParams.SortOrder, StringComparison.OrdinalIgnoreCase)
        ? pagingParams.SortOrder : "DESC";

      return $"{column} {order}";
    }

    public static async Task<IPagedList<T>> ToPagedListAsync<T>(
      this IQueryable<T> source,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default)
    {
      if (source.IsNullOrEmpty())
      {
        return new PagedList<T>(
          new List<T>(),
          pagingParams.PageNumber,
          pagingParams.PageSize,
          0);
      }
      var totalCount = await source.CountAsync(cancellationToken);
      var items = await source
        .OrderBy(pagingParams.GetOrderExpression())
        .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
        .Take(pagingParams.PageSize)
        .ToListAsync(cancellationToken);

      return new PagedList<T>(
        items,
        pagingParams.PageNumber,
        pagingParams.PageSize,
        totalCount);
    }

    public static async Task<IPagedList<T>> ToPagedListAsync<T>(
      this IQueryable<T> source,
      int pageNumber = 1,
      int pageSize = 10,
      string sortColumn = "Id",
      string sortOrder = "DESC",
      CancellationToken cancellationToken = default)
    {
      if (source.IsNullOrEmpty())
      {
        return new PagedList<T>(
          new List<T>(),
          pageNumber,
          pageSize,
          0);
      }
      var totalCount = await source.CountAsync(cancellationToken);
      var items = await source
        .OrderBy($"{sortColumn} {sortOrder}")
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

      return new PagedList<T>(
        items,
        pageNumber,
        pageSize,
        totalCount);
    }
  }
}
