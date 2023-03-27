using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class CommentsController : Controller
  {
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    public CommentsController(
      ICommentRepository commentRepository,
      IMapper mapper)
    {
      _commentRepository = commentRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
      CommentFilterModel model,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 2)
    {
      var commentQuery = _mapper.Map<CommentQuery>(model);

      var comments = await _commentRepository
        .GetPagedCommentsAsync<CommentItem>(
        commentQuery,
        pageNumber,
        pageSize,
        comments => comments.ProjectToType<CommentItem>());

      if (pageNumber > comments.PageCount)
      {
        comments = await _commentRepository
         .GetPagedCommentsAsync<CommentItem>(
         commentQuery,
         pageNumber - 1,
         pageSize,
         comments => comments.ProjectToType<CommentItem>());
      }

      ViewBag.Items = comments;

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Approve(
      int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize)
    {

      var check = await _commentRepository.AprroveCommentAsync(id);
      await Console.Out.WriteLineAsync(check.ToString());
      return Redirect($"{Url
        .ActionLink(
          "Index",
          "Comments",
          new { p = pageNumber, ps = pageSize })}{queryFilter}");
    }


    [HttpGet]
    public async Task<IActionResult> Delete(
      int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize
      )
    {
      await _commentRepository
        .DeleteCommentAsync(id);

      return Redirect($"{Url.ActionLink("Index",
            "Comments", new { p = pageNumber, ps = pageSize })}{queryFilter}");

    }
  }
}
