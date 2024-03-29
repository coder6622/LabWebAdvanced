﻿using System;
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
    Task<Author> GetAuthorByIdAsync(
      int id,
      bool includeDetail = false,
      CancellationToken cancellationToken = default);

    Task<Author> GetCachedAuthorByIdAsync(
      int id,
      bool includeDetail = false,
      CancellationToken cancellationToken = default);

    Task<Author> GetAuthorBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

    Task<Author> GetCachedAuthorBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

    Task<IPagedList<AuthorItem>> GetAllAuthorsAsync(
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default);

    Task<IList<AuthorItem>> GetAllAuthorsAsync(
      CancellationToken cancellationToken = default);

    Task<bool> IsAuthorSlugExist(
      int id,
      string slug,
      CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateAuthor(
      Author author,
      CancellationToken cancellationToken = default);

    Task<IPagedList<Author>> GetNAuthorTopPostsAsync(
      int n,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default);


    Task<IPagedList<T>> GetNAuthorTopPostsAsync<T>(
      int n,
      IPagingParams pagingParams,
      Func<IQueryable<Author>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default);

    Task<IPagedList<Author>> GetPagedAuthorsAsync(
      AuthorQuery authorQuery,
      int pageNumber,
      int pageSize,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default);


    Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
      IPagingParams pagingParams,
      string name = null,
      CancellationToken cancellationToken = default);

    public Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
     AuthorQuery query,
     int pageNumber,
     int pageSize,
     Func<IQueryable<Author>, IQueryable<T>> mapper,
     string sortColumn = "Id",
     string sortOrder = "ASC",
     CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
      Func<IQueryable<Author>, IQueryable<T>> mapper,
      IPagingParams pagingParams,
      string name = null,
      CancellationToken cancellationToken = default);

    public Task<bool> DeleteAuthorAsync(
      int id,
      CancellationToken cancellationToken = default);

    Task<bool> SetImageUrlAsync(
      int authorId, string imageUrl,
      CancellationToken cancellationToken = default);

  }
}
