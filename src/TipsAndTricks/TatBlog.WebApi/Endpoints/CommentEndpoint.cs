using Mapster;
using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Comment;

namespace TatBlog.WebApi.Endpoints
{
  public static class CommentEndpoint
  {
    public static WebApplication MapCommentEndpoints(this WebApplication app)
    {
      var mapGroupBuilder = app.MapGroup("api/comments");

      mapGroupBuilder.MapGet("/", GetComments)
        .WithName("GetComments")
        .Produces<ApiResponse<IPagedList<CommentItem>>>();

      mapGroupBuilder.MapGet("/{id:int}", GetDetailComment)
        .WithName("GetDetailComment")
        .Produces<ApiResponse<CommentDto>>();

      mapGroupBuilder.MapPost("/{postid:int}", AddComment)
        .WithName("AddComment")
        .Produces<ApiResponse<CommentDto>>();

      mapGroupBuilder.MapPut("/{id:int}/{status}", ChangeStatusComment)
        .WithName("ChangeStatusComment")
        .Produces<ApiResponse<string>>();

      mapGroupBuilder.MapDelete("/{id:int}", DeleteComment)
        .WithName("DeleteComment")
        .Produces<ApiResponse<string>>();

      return app;
    }

    private static async Task<IResult> GetComments(
      [AsParameters] CommentFilterModel model,
      ICommentRepository commentRepository,
      IMapper mapper)
    {
      var commentQuery = mapper.Map<CommentQuery>(model);

      var comments = await commentRepository.GetPagedCommentsAsync<CommentItem>(
        commentQuery, model,
        comments => comments.ProjectToType<CommentItem>());

      var paginationResult = new PaginationResult<CommentItem>(comments);
      return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetDetailComment(
      int id,
      ICommentRepository commentRepository,
      IMapper mapper)
    {
      var comment = await commentRepository
        .GetCommentByIdAsync(id, true);


      return comment == null
        ? Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy bình luận có id = {id}"))
        : Results.Ok(ApiResponse.Success(
            mapper.Map<CommentDto>(comment)));
    }

    private static async Task<IResult> AddComment(
      int postId,
      CommentEditModel model,
      IMapper mapper,
      IBlogRepository blogRepository,
      ICommentRepository commentRepository)
    {

      if (await blogRepository.IsPostIdExistedAsync(postId))
      {
        return Results.Ok(ApiResponse.Fail(
          HttpStatusCode.NotFound,
          $"Không tìm thấy bài viết có id = {postId}"));
      }

      var comment = mapper.Map<Comment>(model);
      comment.PostId = postId;
      comment.CommentedDate = DateTime.Now;

      await commentRepository.AddOrUpdateCommentAsync(comment);

      return Results.Ok(ApiResponse.Success(
        mapper.Map<CommentDto>(comment),
        HttpStatusCode.Created));
    }

    private static async Task<IResult> ChangeStatusComment(
      int id,
      bool status,
      ICommentRepository commentRepository)
    {
      return await commentRepository.AprroveCommentAsync(id, status)
         ? Results.Ok(ApiResponse.Success(
             $"Thay đổi trạng thái thành công bình luận id = {id} status = {status}"))
         : Results.Ok(ApiResponse.Fail(
             HttpStatusCode.NotFound,
             $"Không tìm thấy bình luận có id = {id}"));
    }

    private static async Task<IResult> DeleteComment(
      int id,
      ICommentRepository commentRepository)
    {
      return await commentRepository.DeleteCommentAsync(id)
        ? Results.Ok(ApiResponse.Success(
            $"Xóa thành công bình luận id = {id}"))
        : Results.Ok(ApiResponse.Fail(
            HttpStatusCode.NotFound,
            $"Không tìm thấy bình luận id = {id}"));
    }
  }
}
