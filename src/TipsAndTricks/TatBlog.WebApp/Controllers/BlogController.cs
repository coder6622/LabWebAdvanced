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
    private readonly IAuthorRepository _authorRepository;
    public BlogController(
      IBlogRepository blogRepository,
      IAuthorRepository authorRepository)
    {
      _blogRepository = blogRepository;
      _authorRepository = authorRepository;
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

    public async Task<IActionResult> Author(
      string slug,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 5)
    {
      var author = await _authorRepository
        .FindAuthorBySlugAsync(slug);

      var postQuery = new PostQuery()
      {
        PublishedOnly = true,
        AuthorSlug = slug,
        AuthorName = author?.FullName ?? ""
      };

      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);


      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Bài viết của tác giả {postQuery.AuthorName ?? "Ẩn danh"}";

      return View("Index", posts);
    }


    public async Task<IActionResult> Tag(
      string slug,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 5)
    {
      var tag = await _blogRepository
        .GetTagBySlugAsync(slug);

      var postQuery = new PostQuery()
      {
        PublishedOnly = true,
        TagSlug = slug,
        TagName = tag.Name
      };

      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);


      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Các bài viết của thẻ '{postQuery.TagName}'";

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
