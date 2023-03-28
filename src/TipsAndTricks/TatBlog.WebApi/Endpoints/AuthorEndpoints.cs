﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
  {
    public static WebApplication MapAuthorEndpoints(
    this WebApplication app)
    {
      var routeGroupBuilder = app.MapGroup("/api/authors");

      routeGroupBuilder.MapGet("/", GetAuthors)
        .WithName("GetAuthors")
        .Produces<PaginationResult<AuthorItem>>();


      routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
        .WithName("GetAuthorDetails")
        .Produces<AuthorItem>()
        .Produces(404);


      routeGroupBuilder.MapGet("/{id:int}/posts", GetPostsByAuthorId)
        .WithName("GetPostsByAuthorId")
        .Produces<PaginationResult<PostDto>>();


      routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
        .WithName("GetPostsByAuthorSlug")
        .Produces<PaginationResult<PostDto>>();


      routeGroupBuilder.MapPost("/", AddAuthor)
        .WithName("AddAuthor")
        .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
        .Produces(201)
        .Produces(400)
        .Produces(409);

      routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
        .WithName("SetAuthorPicture")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<string>()
        .Produces(400);


      routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
        .WithName("UpdateAnAuthor")
        .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
        .Produces(204)
        .Produces(400)
        .Produces(409);

      routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
        .WithName("DeleteAnAuthor")
        .Produces(204)
        .Produces(404);

      routeGroupBuilder.MapGet("/best/{limit:int}", GetAuthorsBestPost)
        .WithName("GetAuthorsBestPost")
        .Produces<PaginationResult<AuthorItem>>();

      return app;
    }

    private static async Task<IResult> GetAuthors(
      [AsParameters] AuthorFilterModel model,
      IAuthorRepository authorRepository,
      ILogger<IResult> logger)
    {
      logger.LogInformation("Lấy danh sách tác giả");
      var authors = await authorRepository
        .GetPagedAuthorsAsync(model, model.Name);

      logger.LogInformation("Tạo danh sách author item");
      var paginationResult = new PaginationResult<AuthorItem>(authors);

      logger.LogInformation("Trả về kết quả");
      return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetAuthorDetails(
      int id,
      IAuthorRepository authorRepository,
      IMapper mapper,
      ILogger<IResult> logger)
    {
      var author = await authorRepository.GetCachedAuthorByIdAsync(id);

      return author == null
        ? Results.NotFound($"Không tìm thấy tác giả có mã số {id}")
        : Results.Ok(mapper.Map<AuthorItem>(author));
    }

    private static async Task<IResult> GetPostsByAuthorId(
      int id,
      [AsParameters] PagingModel pagingModel,
      IBlogRepository blogRepository,
      ILogger<IResult> logger)
    {
      logger.LogInformation("Tạo điều kiện truy vấn");
      var postQuery = new PostQuery()
      {
        AuthorId = id,
        PublishedOnly = true,
      };

      logger.LogInformation("Lấy danh sách post và map sang PostDto");
      var posts = await blogRepository.GetPagedPostsAsync<PostDto>(
        postQuery, pagingModel,
        posts => posts.ProjectToType<PostDto>());

      var paginationResult = new PaginationResult<PostDto>(posts);

      return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetPostsByAuthorSlug(
        [FromRoute] string slug,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository,
        ILogger<IResult> logger)
    {
      logger.LogInformation("Tạo điều kiện truy vấn");
      var postQuery = new PostQuery()
      {
        AuthorSlug = slug,
        PublishedOnly = true,
      };

      logger.LogInformation("Lấy danh sách");
      var posts = await blogRepository.GetPagedPostsAsync<PostDto>(
        postQuery, pagingModel,
        posts => posts.ProjectToType<PostDto>());

      var paginationResult = new PaginationResult<PostDto>(posts);

      logger.LogInformation("Trả về kết quả");
      return Results.Ok(paginationResult);
    }

    private static async Task<IResult> AddAuthor(
        AuthorEditModel model,
        IAuthorRepository authorRepository,
        IMapper mapper,
        ILogger<IResult> logger)
    {
      logger.LogInformation("Kiểm tra dữ liệu");
      if (await authorRepository
        .IsAuthorSlugExist(0, model.UrlSlug))
      {
        return Results.Conflict(
          $"Slug '{model.UrlSlug}' đã được sử dụng");
      }

      var author = mapper.Map<Author>(model);
      await authorRepository.AddOrUpdateAuthor(author);

      logger.LogInformation("Trả về author vừa tạo");
      return Results.CreatedAtRoute("GetAuthorDetails", new { author.Id },
        mapper.Map<AuthorItem>(author));
    }

    private static async Task<IResult> SetAuthorPicture(
       int id,
       IFormFile imagefile,
       IAuthorRepository authorRepository,
       IMediaManager mediaManager,
       ILogger<IResult> logger)
    {
      logger.LogInformation("Lưu file");
      var imageUrl = await mediaManager.SaveFileAsync(
        imagefile.OpenReadStream(),
        imagefile.FileName, imagefile.ContentType);

      if (string.IsNullOrWhiteSpace(imageUrl))
      {
        return Results.BadRequest("Không lưu được tập tin");
      }
      await authorRepository.SetImageUrlAsync(id, imageUrl);
      return Results.Ok(imageUrl);
    }

    private static async Task<IResult> UpdateAuthor(
      int id, AuthorEditModel model,
      IAuthorRepository authorRepository,
      IMapper mapper)
    {
      if (await authorRepository
        .IsAuthorSlugExist(id, model.UrlSlug))
      {
        return Results.Conflict(
          $"Slug '{model.UrlSlug}' đã được sử dụng");
      }

      var author = mapper.Map<Author>(model);
      author.Id = id;

      return await authorRepository.AddOrUpdateAuthor(author)
        ? Results.NoContent()
        : Results.NotFound();
    }

    private static async Task<IResult> DeleteAuthor(
      int id,
      IAuthorRepository authorRepository)
    {
      return await authorRepository.DeleteAuthorAsync(id)
        ? Results.NoContent()
        : Results.NotFound($"Không tìm thấy tác giả có id = {id}");
    }

    private static async Task<IResult> GetAuthorsBestPost(
      int limit,
      [AsParameters] PagingModel pagingModel,
      IAuthorRepository authorRepository)
    {
      var authors = await authorRepository
        .GetNAuthorTopPostsAsync<AuthorItem>(limit, pagingModel, authors => authors.ProjectToType<AuthorItem>());

      var paginationResult = new PaginationResult<AuthorItem>(authors);

      return Results.Ok(paginationResult);

    }
  }
}
