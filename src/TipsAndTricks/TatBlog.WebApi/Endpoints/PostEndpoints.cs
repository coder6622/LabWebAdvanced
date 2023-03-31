using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Post;
using TatBlog.WinApp;

namespace TatBlog.WebApi.Endpoints
{
  public class PostEndpoints : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      var routeGroupBuilder = app.MapGroup("/api/posts");

      routeGroupBuilder.MapGet("/", GetPosts)
        .WithName("GetPosts")
        .Produces<ApiResponse<PaginationResult<PostDto>>>();

      routeGroupBuilder.MapGet("featured/{limit:int}", GetNPostsTopCount)
        .WithName("GetPostsFeatured")
        .Produces<ApiResponse<IList<PostDto>>>();

      routeGroupBuilder.MapGet("/random/{limit:int}", GetNPostsRandom)
        .WithName("GetNPostsRandom")
        .Produces<ApiResponse<IList<PostDto>>>();


      routeGroupBuilder.MapGet("archives/{limit:int}", GetNPostsMonthly)
        .WithName("GetNPostsMonthly")
        .Produces<ApiResponse<IList<AmountPostItemByMonth>>>();

      routeGroupBuilder.MapGet("{id:int}", GetPostDetail)
        .WithName("GetPostDetail")
        .Produces<ApiResponse<PostDetail>>();

      routeGroupBuilder.MapGet(
          "/byslug/{slug:regex(^[a-z0-9_-]+$)}",
          GetPostDetailBySlug)
        .WithName("GetPostDetailBySlug")
        .Produces<ApiResponse<PostDetail>>();

      routeGroupBuilder.MapPost("/", AddPost)
        .WithName("AddPost")
        .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
        .Produces<ApiResponse<PostDto>>();

      routeGroupBuilder.MapPost("{id:int}/picture", SetImagePost)
        .WithName("SetImagePost")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ApiResponse<string>>();


      routeGroupBuilder.MapPut("/", UpdatePost)
        .WithName("UpdatePost")
        .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
        .Produces<ApiResponse<string>>();


      routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
        .WithName("DeletePost")
        .Produces<ApiResponse<string>>();

      routeGroupBuilder.MapGet("/{id:int}/comments", GetCommentsByPostId)
        .WithName("GetCommentsByPostId")
        .Produces<ApiResponse<IList<Comment>>>();
    }

    private static async Task<IResult> GetPosts(
      [AsParameters] PostFilterModel model,
      IBlogRepository blogRepository,
      IMapper mapper,
      ILogger<IResult> logger)
    {

      logger.LogInformation("Tạo điều kiện truy vấn");
      var postQuery = mapper.Map<PostQuery>(model);

      logger.LogInformation("Lấy danh sách posts");
      var posts = await blogRepository
        .GetPagedPostsAsync<PostDto>(
          postQuery, model,
          posts => posts.ProjectToType<PostDto>());

      logger.LogInformation("Tạo pagination result");
      var paginationResult = new
        PaginationResult<PostDto>(posts);

      logger.LogInformation("Trả về kết quả");
      return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetNPostsTopCount(
      int limit,
      IBlogRepository blogRepository,
      ILogger<IResult> logger)
    {
      logger.LogInformation("Lấy danh sách post top view");
      var posts = await blogRepository
        .GetNPostsTopCountAsync(
          limit,
          posts => posts.ProjectToType<PostItem>());

      return Results.Ok(ApiResponse.Success(posts));
    }

    private static async Task<IResult> GetNPostsRandom(
      int limit,
      IBlogRepository blogRepository,
      ILogger<IResult> logger)
    {
      var posts = await blogRepository
        .GetRandomNPosts(
          limit,
          posts => posts.ProjectToType<PostDto>());

      return Results.Ok(ApiResponse.Success(posts));
    }

    private static async Task<IResult> GetNPostsMonthly(
      int limit,
      IBlogRepository blogRepository,
      ILogger<IResult> logger)
    {
      var monthsWithAmountPosts = await blogRepository.CountPostsInNMonthsAsync(limit);

      return Results.Ok(ApiResponse.Success(monthsWithAmountPosts));
    }

    private static async Task<IResult> GetPostDetail(
      int id,
      IMapper mapper,
      IBlogRepository blogRepository)
    {
      var post = await blogRepository.GetPostByIdAsync(id, true);

      return post == null
        ? Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy id = {id}"))
        : Results.Ok(ApiResponse.Success(
            mapper.Map<PostDetail>(post)));
    }

    private static async Task<IResult> GetPostDetailBySlug(
      [FromRoute] string slug,
      IBlogRepository blogRepository,
      IMapper mapper)
    {
      var post = await blogRepository.GetPostBySlugAsync(slug, true);

      return post == null
        ? Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy bài viết '{slug}'"))
        : Results.Ok(ApiResponse.Success(
            mapper.Map<PostDetail>(post)));
    }

    private static async Task<IResult> AddPost(
      PostEditModel model,
      IMapper mapper,
      IBlogRepository blogRepository,
      IAuthorRepository authorRepository,
      IMediaManager mediaManager)
    {

      if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Slug '{model.UrlSlug}' đã được sử dụng"));
      }

      if (await authorRepository
        .GetAuthorByIdAsync(model.AuthorId) == null)
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Không tìm thấy tác giả id = {model.AuthorId}"));

      }

      if (await blogRepository
        .GetCategoryByIdAsync(model.CategoryId) == null)
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Không tìm thấy chủ đề id = {model.CategoryId}"));
      }

      var post = mapper.Map<Post>(model);
      post.PostedDate = DateTime.Now;

      await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags());

      return Results.Ok(ApiResponse.Success(
        mapper.Map<PostDto>(post),
        HttpStatusCode.Created));
    }

    private static async Task<IResult> SetImagePost(
      int id,
      IFormFile imageFile,
      IBlogRepository blogRepository,
      IMediaManager mediaManager,
      ILogger<IResult> logger)
    {

      logger.LogInformation($"Lưu file ảnh cho bài viết id = {id}");

      var imageUrl = await mediaManager.SaveFileAsync(
        imageFile.OpenReadStream(),
        imageFile.FileName, imageFile.ContentType);

      if (string.IsNullOrWhiteSpace(imageUrl))
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.BadRequest,
            "Không lưu được tập tin"));
      }

      await blogRepository.SetImageUrlPostAsync(id, imageUrl);

      return Results.Ok(ApiResponse.Success(imageUrl));
    }

    private static async Task<IResult> UpdatePost(
         int id,
         PostEditModel model,
         IMapper mapper,
         IBlogRepository blogRepository,
         IAuthorRepository authorRepository,
         IMediaManager mediaManager)
    {

      var post = await blogRepository.GetPostByIdAsync(id);

      if (post == null)
      {
        return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.NotFound,
                $"Không tìm thấy bài viết có id = {id}"));
      }

      if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Slug '{model.UrlSlug}' đã được sử dụng"));
      }

      if (await authorRepository
        .GetAuthorByIdAsync(model.AuthorId) == null)
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Không tìm thấy tác giả id = {model.AuthorId}"));

      }

      if (await blogRepository
        .GetCategoryByIdAsync(model.CategoryId) == null)
      {
        return Results.Ok(
          ApiResponse.Fail(
            HttpStatusCode.Conflict,
            $"Không tìm thấy chủ đề id = {model.CategoryId}"));
      }

      mapper.Map(model, post);
      post.Id = id;
      post.ModifiedDate = DateTime.Now;

      return await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags())
         ? Results.Ok(ApiResponse.Success(
             $"Thay đổi bài viết id = {id} thành công"))
         : Results.Ok(ApiResponse.Fail(
             HttpStatusCode.NotFound,
             $"Không tìm thấy bài viết có id = {id}"));
    }

    private static async Task<IResult> DeletePost(
      int id,
      IBlogRepository blogRepository)
    {
      return await blogRepository.DeletePostByIdAsync(id)
        ? Results.Ok(ApiResponse.Success(
            $"Xóa thành công id = {id}"))
        : Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy bài viết id = {id}"));
    }

    private static async Task<IResult> GetCommentsByPostId(
      int id,
      ICommentRepository commentRepository)
    {
      var comments = await commentRepository
        .GetCommentsByPostIdAsync(id);

      return Results.Ok(ApiResponse.Success(comments));
    }

  }
}
