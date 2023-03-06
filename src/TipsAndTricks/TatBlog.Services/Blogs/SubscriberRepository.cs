﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
  public class SubscriberRepository : ISubscriberRepository
  {
    private readonly BlogDbContext _context;
    public SubscriberRepository(BlogDbContext context)
    {
      _context = context;
    }
    public async Task<bool> BlockSubscriberAsync(
      int id,
      string reason,
      string notes,
      CancellationToken cancellationToken = default)
    {
      Subscriber subscriber = await GetSubscriberByIdAsync(id, cancellationToken);
      if (subscriber == null)
      {
        return false;
      }

      subscriber.UnsubscribedDate ??= DateTime.Now;
      subscriber.UnsubscribedCausal = reason;
      subscriber.AdminNotes = notes;
      subscriber.FlagIsBlockSubByAdmin = true;

      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }

    public async Task<bool> DeleteSubscriberAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Subscriber>()
        .Where(s => s.Id == id)
        .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<Subscriber> GetSubscriberByEmailAsync(
      string email,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Subscriber>()
        .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
    }

    public async Task<Subscriber> GetSubscriberByIdAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Subscriber>()
        .FindAsync(id, cancellationToken);
    }

    public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(
      IPagingParams pagingParams,
      string keyword,
      bool unsubscribed = false,
      bool involuntary = false,
      CancellationToken cancellationToken = default
    )
    {
      IQueryable<Subscriber> subscribersQuery = _context.Set<Subscriber>();

      if (!string.IsNullOrEmpty(keyword))
      {
        subscribersQuery = subscribersQuery
          .Where(s => s.Email.Contains(keyword)
          || s.AdminNotes.Contains(keyword)
          || s.UnsubscribedCausal.Contains(keyword));
      }

      if (unsubscribed)
      {
        subscribersQuery = subscribersQuery.Where(s => s.UnsubscribedDate != null);
      }

      if (involuntary)
      {
        subscribersQuery = subscribersQuery
          .Where(s => s.FlagIsBlockSubByAdmin
            && s.UnsubscribedDate != null);
      }
      return await subscribersQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<Subscriber> SubscribeAsync(
      string email,
      CancellationToken cancellationToken = default)
    {
      Subscriber subscriber = await GetSubscriberByEmailAsync(email, cancellationToken);
      if (subscriber != null)
      {
        // not unsubscribed or lock subscriber by admin
        if (subscriber.UnsubscribedDate == null || subscriber.FlagIsBlockSubByAdmin)
        {
          return subscriber;
        }

        subscriber.SubscribedDate = DateTime.Now;
        subscriber.UnsubscribedDate = null;
        subscriber.UnsubscribedCausal = null;
        subscriber.FlagIsBlockSubByAdmin = false;
      }
      else
      {
        subscriber = new Subscriber()
        {
          Email = email,
          SubscribedDate = DateTime.Now
        };

        _context.Set<Subscriber>().Add(subscriber);
      }


      await _context.SaveChangesAsync(cancellationToken);
      return subscriber;
    }

    public async Task<bool> UnsubscribeAsync(
      string email,
      string reason,
      bool voluntary,
      CancellationToken cancellationToken = default)
    {
      Subscriber subscriber = await GetSubscriberByEmailAsync(email, cancellationToken);

      if (subscriber == null)
      {
        return false;
      }
      if (subscriber.UnsubscribedDate != null)
      {
        return true;
      }

      subscriber.UnsubscribedDate = DateTime.Now;
      subscriber.UnsubscribedCausal = reason;
      subscriber.FlagIsBlockSubByAdmin = !voluntary;

      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }
}
