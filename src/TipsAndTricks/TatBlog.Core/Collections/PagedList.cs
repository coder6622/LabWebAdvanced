using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Collections
{
  public class PagedList<T> : IPagedList<T>
  {
    private readonly List<T> _subset = new();

    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="items"></param>
    /// <param name="pageNumber">Vị trí của trang hiện tại</param>
    /// <param name="pageSize">Số lượng phần tử tối đa trên 1 trang</param>
    /// <param name="totalCount">Số phần tử trả về tử truy vấn</param>
    public PagedList(IList<T> items, int pageNumber, int pageSize, int totalCount)
    {
      PageNumber = pageNumber;
      PageSize = pageSize;
      TotalItemCount = totalCount;

      _subset.AddRange(items);
    }
    public T this[int index] => _subset[index];

    public int Count => _subset.Count;

    // Tổng số trang
    public int PageCount
    {
      get
      {
        if (PageSize == 0)
        {
          return 0;
        }
        var total = TotalItemCount / PageSize;
        if (TotalItemCount % PageSize > 0)
        {
          total++;
        }
        return total;
      }
    }

    public int TotalItemCount { get; }

    public int PageIndex { get; set; }

    public int PageNumber { get => PageIndex + 1; set => PageIndex = value - 1; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => (PageIndex > 0);

    public bool HasNextPage => (PageIndex < (PageCount - 1));

    public bool IsFirstPage => (PageIndex <= 0);

    public bool IsLastPage => (PageIndex >= (PageCount - 1));

    public int FirstItemIndex => (PageIndex * PageSize) + 1;

    public int LastItemIndex => Math.Min(TotalItemCount, ((PageIndex * PageSize) + PageSize));

    public IEnumerator<T> GetEnumerator()
    {
      return _subset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
