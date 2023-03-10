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
    private readonly ICommentRepository _commentRepository;
    public BlogController(
      IBlogRepository blogRepository,
      IAuthorRepository authorRepository,
      ICommentRepository commentRepository)
    {
      _blogRepository = blogRepository;
      _authorRepository = authorRepository;
      _commentRepository = commentRepository;
    }

    public async Task<IActionResult> Index(
      [FromQuery(Name = "k")] string keyword = null,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 5)
    {

      var postQuery = new PostQuery()
      {
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
        CategorySlug = slug,
      };

      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);


      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Các bài viết của chủ đề '{category.Name}'";

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
        AuthorSlug = slug,
      };

      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);


      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Các bài viết của tác giả '{author.FullName ?? "Ẩn danh"}'";

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
        TagSlug = slug,
      };

      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);


      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Các bài viết của thẻ '{tag.Name}'";

      return View("Index", posts);
    }

    public async Task<IActionResult> Post(
      int year,
      int month,
      int day,
      string slug)
    {
      var post = await _blogRepository.GetPostsAsync(year, month, slug);

      if (post == null)
      {
        ViewBag.Message = $"Không tìm thấy bài viết '{slug}'";
        return View("Error");
      }

      if (!post.Published)
      {
        ViewBag.Message = $"Bài viết '{slug}' chưa công khai";
        return View("Error");
      }

      ViewBag.Title = post.Title;
      ViewBag.Comments = await _commentRepository
        .GetAllCommentsIsApprovedByIdPost(post.Id);
      await _blogRepository.IncreaseViewCountAsync(post.Id);

      return View("PostDetail", post);
    }

    public async Task<IActionResult> Archive(
      [FromQuery(Name = "month")] int month,
      [FromQuery(Name = "year")] int year,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 5)
    {

      var postQuery = new PostQuery()
      {
        PostedMonth = month,
        PostedYear = year
      };

      IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
      var posts = await _blogRepository
        .GetPagedPostsAsync(postQuery, pagingParams);

      ViewBag.PostQuery = postQuery;
      ViewBag.Title = $"Các bài viết trong tháng {postQuery.PostedMonth} năm {postQuery.PostedYear}";

      return View("Index", posts);
    }

    public IActionResult Contact()
    {
      return View();
    }

    public IActionResult About()
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
