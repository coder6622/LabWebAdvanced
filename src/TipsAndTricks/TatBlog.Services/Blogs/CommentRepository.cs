﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
  public class CommentRepository : ICommentRepository
  {
    private readonly BlogDbContext _context;
    public CommentRepository(BlogDbContext context)
    {
      _context = context;
    }


    public async Task<int> CountCommentApprovedByIdPost(int postId, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .CountAsync(c => c.PostId == postId & c.IsApproved ?? false, cancellationToken);
    }

    //public async Task<Comment> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
    //{
    //  if (comment.Id > 0)
    //  {
    //    Comment commentEditted = await GetCommentByIdAsync(comment.Id, cancellationToken);
    //    if (comment == null)
    //    {
    //      await Console.Out.WriteLineAsync("Khong tim thay comment");
    //      return comment;
    //    }
    //    _context.Entry(commentEditted).CurrentValues.SetValues(comment);
    //  }
    //  else
    //  {
    //    _context.Set<Comment>().Add(comment);
    //  }

    //  await _context.SaveChangesAsync(cancellationToken);
    //  return comment;
    //}

    public async Task<bool> DeleteCommentAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.Id == id)
        .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<IList<Comment>> GetAllCommentsIsApprovedByIdPost(int postId, CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.PostId == postId & c.IsApproved ?? false)
        .ToListAsync(cancellationToken);
    }

    public async Task<Comment> GetCommentByIdAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>().FindAsync(id, cancellationToken);
    }

    public async Task<IList<Comment>> GetCommentsByEmailAsync(
      string email,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.Email == email)
        .ToListAsync(cancellationToken);
    }

    public async Task<IList<Comment>> GetCommentsByPostIdAsync(
      int postId,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Comment>()
        .Where(c => c.PostId == postId)
        .ToListAsync(cancellationToken);
    }

    public async Task<bool> VerifyCommentAsync(
      int id,
      bool isApprove = true,
      CancellationToken cancellationToken = default)
    {
      Comment comment = await GetCommentByIdAsync(id, cancellationToken);
      if (comment == null)
      {
        return false;
      }

      comment.IsApproved = isApprove;
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }
}
