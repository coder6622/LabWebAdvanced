using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Models;

namespace TatBlog.WebApp.Controllers
{
  public class BlogController : Controller
  {

    private readonly IBlogRepository _blogRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    public BlogController(
      IBlogRepository blogRepository,
      IAuthorRepository authorRepository,
      ICommentRepository commentRepository,
      IMapper mapper)
    {
      _blogRepository = blogRepository;
      _authorRepository = authorRepository;
      _commentRepository = commentRepository;
      _mapper = mapper;
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
      [FromQuery(Name = "ps")] int pageSize = 2)
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

    public async Task<IActionResult> PostById(
      int id)
    {
      var post = await _blogRepository.FindPostByIdAsync(id, true);

      if (post == null)
      {
        ViewBag.Message = $"Không tìm thấy bài viết ";
        return View("Error");
      }

      if (!post.Published)
      {
        ViewBag.Message = $"Bài viết '{post.UrlSlug}' chưa công khai";
        return View("Error");
      }
      ViewBag.Title = post.Title;

      ViewBag.Comments = await _commentRepository
        .GetAllCommentsIsApprovedByIdPostAsync(post.Id);



      var commentModel = new CommentEditModel()
      {
        PostId = post.Id
      };

      ViewBag.Post = post;


      await _blogRepository.IncreaseViewCountAsync(post.Id);

      return View("PostDetail", commentModel);

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
        .GetAllCommentsIsApprovedByIdPostAsync(post.Id);

      var commentModel = new CommentEditModel()
      {
        PostId = post.Id
      };
      ViewBag.CommentModel = commentModel;
      await Console.Out.WriteLineAsync(commentModel.ToString());


      await _blogRepository.IncreaseViewCountAsync(post.Id);

      return View("PostDetail", post);
    }

    [HttpPost]
    public async Task<IActionResult> PostById(
        [FromServices] IValidator<CommentEditModel> commentValidator,
        CommentEditModel model)
    {

      var validationResult = await commentValidator
  .ValidateAsync(model);

      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
      }

      if (!ModelState.IsValid)
      {
        //return RedirectToPage("/blog/post",
        //  new { year, month, slug });
        //return Redirect($"{Url.ActionLink("PostById",
        //      "Blog", new { Area = "", id = model.PostId })}");
        return View("PostDetail", model);
      }

      var comment = model.Id > 0
              ? await _commentRepository.GetCommentByIdAsync(model.Id)
              : null;

      if (comment == null)
      {
        comment = _mapper.Map<Comment>(model);
        comment.Id = 0;
        comment.CommentedDate = DateTime.Now;
      }
      else
      {
        _mapper.Map(model, comment);
      }

      await _commentRepository.AddOrUpdateCommentAsync(comment);

      return Redirect($"{Url.ActionLink("PostById",
              "Blog", new { Area = "", id = model.PostId })}");
    }

    public async Task<IActionResult> Archive(
      int month,
      int year,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 1)
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
