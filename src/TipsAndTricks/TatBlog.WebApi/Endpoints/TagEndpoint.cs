using Carter;
using Mapster;
using MapsterMapper;
using System.Net;
using System.Runtime.CompilerServices;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Endpoints
{
  public class TagEndpoint : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      var routeGroupBuilder = app.MapGroup("api/tags");

      routeGroupBuilder.MapGet("/", GetTags)
        .WithName("GetTags")
        .Produces<ApiResponse<IPagedList<TagItem>>>();

      routeGroupBuilder.MapGet("/{id:int}", GetTagDetail)
        .WithName("GetTagDetail")
        .Produces<ApiResponse<TagItem>>();

      routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}", GetTagBySlug)
        .WithName("GetTagBySlug")
        .Produces<ApiResponse<TagItem>>();

      routeGroupBuilder.MapPost("/", AddTag)
        .WithName("AddTag")
        .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
        .Produces<ApiResponse<TagDto>>();

      routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
        .WithName("UpdateTag")
        .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
        .Produces<ApiResponse<string>>();

      routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
        .WithName("DeleteTag")
        .Produces<ApiResponse<string>>();
    }

    private static async Task<IResult> GetTags(
      [AsParameters] TagFilterModel model,
      IMapper mapper,
      IBlogRepository blogRepository)
    {

      var tagQuery = mapper.Map<TagQuery>(model);


      var tags = await blogRepository.GetPagedTagsAsync(
        tagQuery, model,
        tags => tags.ProjectToType<TagItem>());

      var paginationResult = new PaginationResult<TagItem>(tags);

      return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetTagDetail(
      int id,
      IBlogRepository blogRepository,
      IMapper mapper)
    {

      var tag = await blogRepository.GetTagByIdAsync(id, true);

      return tag == null
        ? Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy id = {id}"))
        : Results.Ok(ApiResponse.Success(
            mapper.Map<TagItem>(tag)));
    }


    private static async Task<IResult> GetTagBySlug(
      string slug,
      IBlogRepository blogRepository,
      IMapper mapper)
    {

      var tag = await blogRepository.GetTagBySlugAsync(slug);

      return tag == null
        ? Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy thẻ có slug = {slug}"))
        : Results.Ok(ApiResponse.Success(
            mapper.Map<TagItem>(tag)));
    }


    private static async Task<IResult> AddTag(
      TagEditModel model,
      IBlogRepository blogRepository,
      IMapper mapper)
    {

      if (await blogRepository.IsTagSlugExistAsync(0, model.UrlSlug))
      {
        return Results.Ok(ApiResponse.Fail(
          HttpStatusCode.Conflict,
          $"Slug '{model.UrlSlug}' đã được sử dụng"));
      }

      var tag = mapper.Map<Tag>(model);

      await blogRepository.AddOrUpdateTagAsync(tag);

      return Results.Ok(
        ApiResponse.Success(
           mapper.Map<TagDto>(tag),
           HttpStatusCode.Created));
    }

    private static async Task<IResult> UpdateTag(
      int id,
      TagEditModel model,
      IBlogRepository blogRepository,
      IMapper mapper)
    {

      var tag = await blogRepository.GetTagByIdAsync(id);

      if (tag == null)
      {
        return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.NotFound,
                $"Không tìm thấy thẻ có id = {id}"));
      }

      if (await blogRepository.IsTagSlugExistAsync(id, model.UrlSlug))
      {
        return Results.Ok(ApiResponse.Fail(
          HttpStatusCode.Conflict,
          $"Slug '{model.UrlSlug}' đã được sử dụng"));
      }

      mapper.Map(model, tag);
      tag.Id = id;

      return await blogRepository.AddOrUpdateTagAsync(tag)
          ? Results.Ok($"Thay đổi thẻ có id = {id} thành công")
          : Results.Ok(
            ApiResponse.Fail(
              HttpStatusCode.NotFound,
              $"Không tìm thấy thẻ có id = {id}"));
    }

    private static async Task<IResult> DeleteTag(
      int id,
      IBlogRepository blogRepository)
    {
      return await blogRepository.RemoveTagByIdAsync(id)
        ? Results.Ok(ApiResponse.Success(
            $"Xóa thành công thẻ id = {id}"))
        : Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy thẻ có id = {id}"));

    }
  }
}
