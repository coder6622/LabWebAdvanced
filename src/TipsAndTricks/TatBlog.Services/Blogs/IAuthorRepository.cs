using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
  public interface IAuthorRepository
  {
    Task<Author> FindAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Author> FindAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IPagedList<AuthorItem>> GetAllAuthor(IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task AddOrUpdateAuthor(Author author, CancellationToken cancellationToken = default);

    Task<IPagedList<Author>> GetNAuthorTopPosts(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default);
  }
}
