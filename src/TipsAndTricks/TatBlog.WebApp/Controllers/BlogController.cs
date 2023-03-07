using Microsoft.AspNetCore.Mvc;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
  public class BlogController : Controller
  {

    private readonly IBlogRepository _blogRepository;
    public BlogController(IBlogRepository blogRepository)
    {
      _blogRepository = blogRepository;
    }
    public IActionResult Index()
    {
      ViewBag.CurrentTime = DateTime.Now
        .ToString("HH:mm:ss");
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> Index(
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 10)
    {

      var postQuery = new PostQuery()
      {
        PublishedOnly = true,
      };


      IPagingParams pagingParams = new PagingParams()
      {
        PageNumber = pageNumber,
        PageSize = pageSize,
        SortColumn = "PostedDate",
        SortOrder = "DESC"
      };

      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);

      ViewBag.PostQuery = postQuery;

      return View(posts);
    }

    public IActionResult About()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }

    public IActionResult Rss()
    {
      return Content("Nội dung sẽ được cập nhật");
    }
  }
}
