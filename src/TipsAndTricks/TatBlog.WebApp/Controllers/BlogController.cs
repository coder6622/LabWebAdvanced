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
      [FromQuery(Name = "k")] string keyword = null,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 5)
    {

      var postQuery = new PostQuery()
      {
        PublishedOnly = true,

        Keyword = keyword,
      };


      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);

      ViewBag.PostQuery = postQuery;
      ViewBag.Title = "Trang chủ";

      return View(posts);
    }

    public async Task<IActionResult> Category(
      string slug,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 5)
    {

      var category = await _blogRepository
        .FindCategoryBySlugAsync(slug);
      var postQuery = new PostQuery()
      {
        PublishedOnly = true,
        CategorySlug = slug,
        CategoryName = category.Name
      };



      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);


      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Chủ đề {postQuery.CategoryName}";

      return View("Index", posts);
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

    private IPagingParams CreatePagingParamsPost(
      int pageNumber = 1,
      int pageSize = 5,
      string sortColumn = "PostedDate",
      string sortOrder = "DESC")
    {
      return new PagingParams()
      {
        PageNumber = pageNumber,
        PageSize = pageSize,
        SortColumn = sortColumn,
        SortOrder = sortOrder
      };
    }
  }
}
