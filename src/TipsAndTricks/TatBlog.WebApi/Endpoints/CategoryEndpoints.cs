﻿using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Endpoints
{
  public static class CategoryEndpoints
  {
    public static WebApplication MapCategoryEndpoints(
      this WebApplication app)
    {

      var routeGroupBuilder = app.MapGroup("/api/categories");

      routeGroupBuilder.MapGet("/", GetCategories)
        .WithName("GetCategories")
        .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

      routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
        .WithName("GetCategoryDetails")
        .Produces<ApiResponse<CategoryItem>>();

      routeGroupBuilder.MapGet(
          "{slug:regex(^[a-z0-9_-]+$)}/posts",
          GetPostsByCategorySlug)
        .WithName("GetPostByCategorySlug")
        .Produces<ApiResponse<PaginationResult<CategoryDto>>>();

      routeGroupBuilder.MapPost("/", AddCategory)
        .WithName("AddCategory")
        .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
        .Produces<ApiResponse<CategoryItem>>();

      routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
        .WithName("UpdateCategory")
        .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
        .Produces<ApiResponse<string>>();

      routeGroupBuilder.MapDelete("/{id:int}", DeleteAnCategory)
        .WithName("DeleteAnCategory")
        .Produces<ApiResponse<string>>();

      return app;
    }


    private static async Task<IResult> GetCategories(
      [AsParameters] CategoryFilterModel model,
      IBlogRepository blogRepository,
      ILogger<IResult> logger,
      IMapper mapper)
    {

      var categoryQuery = mapper.Map<CategoryQuery>(model);
      logger.LogInformation("Lấy danh sách chủ đề");

      var categories = await blogRepository
        .GetPagedCategoriesAsync<CategoryItem>(categoryQuery, model,
          categories => categories.ProjectToType<CategoryItem>());

      logger.LogInformation("Tạo danh sách category item");

      var paginationResult = new PaginationResult<CategoryItem>(categories);

      return Results.Ok(ApiResponse.Success(paginationResult));
    }


    private static async Task<IResult> GetCategoryDetails(
      int id,
      IBlogRepository blogRepository,
      IMapper mapper,
      ILogger<IResult> logger)
    {
      var category = await blogRepository
              .GetCachedCategoryByIdAsync(id, true);

      return category == null
        ? Results.Ok(
            ApiResponse.Fail(
              HttpStatusCode.NotFound,
              $"Không tìm thấy chủ đề có id = {id}"))
        : Results.Ok(
            ApiResponse.Success(
              mapper.Map<CategoryItem>(category)));
    }

    private static async Task<IResult> GetPostsByCategorySlug(
      string slug,
      [AsParameters] PagingModel pagingModel,
      IBlogRepository blogRepository,
      ILogger<IResult> logger)
    {

      logger.LogInformation("Tạo điều kiện truy vấn");

      var postQuery = new PostQuery()
      {
        CategorySlug = slug,
        PublishedOnly = true
      };

      logger.LogInformation("Lấy danh sách");
      var posts = await blogRepository.GetPagedPostsAsync<PostDto>(
        postQuery, pagingModel,
        posts => posts.ProjectToType<PostDto>());

      logger.LogInformation("Trả về kết quả");

      var paginationResult = new PaginationResult<PostDto>(posts);

      return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> AddCategory(
      CategoryEditModel model,
      IBlogRepository blogRepository,
      IMapper mapper,
      ILogger<IResult> logger)
    {
      logger.LogInformation("Kiểm tra dữ liệu");

      if (await blogRepository.IsCategorySlugExistAsync(0, model.UrlSlug))
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Slug '{model.UrlSlug}' đã dược sử dụng"));
      }

      var category = mapper.Map<Category>(model);

      await blogRepository.AddOrUpdateCategoryAsync(category);

      logger.LogInformation("Trả về category vừa tạo");

      return Results.Ok(
          ApiResponse.Success(
            mapper.Map<CategoryItem>(category),
            HttpStatusCode.Created));
    }

    private static async Task<IResult> UpdateCategory(
      int id,
      CategoryEditModel model,
      IBlogRepository blogRepository,
      IMapper mapper,
      ILogger<IResult> logger)
    {
      if (await blogRepository.IsCategorySlugExistAsync(id, model.UrlSlug))
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Slug '{model.UrlSlug} đã được sử dụng'"));
      }

      var category = mapper.Map<Category>(model);
      category.Id = id;

      return await blogRepository.AddOrUpdateCategoryAsync(category)
        ? Results.Ok(
            ApiResponse.Success($"Đã thay đổi thành công id = {id}"))
        : Results.Ok(
            ApiResponse.Fail(
              HttpStatusCode.NotFound,
              $"Không tìm thấy id = {id}"));
    }

    private static async Task<IResult> DeleteAnCategory(
      int id,
      IBlogRepository blogRepository,
      ILogger<IResult> logger)
    {
      logger.LogInformation("Gọi hàm xóa");
      return await blogRepository.DeleteCategoryByIdAsync(id)
              ? Results.Ok(
                  ApiResponse.Success($"Đã xóa thành công id = {id}"))
              : Results.Ok(
                  ApiResponse.Fail(
                    HttpStatusCode.NotFound,
                    $"Không tìm thấy chủ đề có id={id}"));
    }
  }
}
