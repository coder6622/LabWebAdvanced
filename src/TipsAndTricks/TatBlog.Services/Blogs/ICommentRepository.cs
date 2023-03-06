using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
  public interface ICommentRepository
  {
    Task<Comment> GetCommentByIdAsync(
      int id,
      CancellationToken cancellationToken = default);
    Task<IList<Comment>> GetCommentsByPostIdAsync(
      int postId,
      CancellationToken cancellationToken = default);

    Task<IList<Comment>> GetCommentsByEmailAsync(
      string email,
      CancellationToken cancellationToken = default);
    //Task<Comment> AddOrUpdateCommentAsync(
    //  Comment comment,
    //  CancellationToken cancellationToken = default);
    Task<bool> DeleteCommentAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<bool> VerifyCommentAsync(
      int id,
      bool isApprove = true,
      CancellationToken cancellationToken = default);

  }
}
