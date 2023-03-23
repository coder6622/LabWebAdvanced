using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Components;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class PostsController : Controller
  {
    private readonly ILogger<PostsController> _logger;
    private readonly IBlogRepository _blogRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMediaManager _mediaManager;
    private readonly IMapper _mapper;
    public PostsController(
      ILogger<PostsController> logger,
      IBlogRepository blogRepository,
      IAuthorRepository authorRepository,
      IMediaManager mediaManager,
      IMapper mapper
      )
    {
      _logger = logger;
      _blogRepository = blogRepository;
      _authorRepository = authorRepository;
      _mediaManager = mediaManager;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
      PostFilterModel model,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 2
      )
    {
      _logger.LogInformation("Tạo điều kiện truy vấn");
      var postQuery = _mapper.Map<PostQuery>(model);

      _logger.LogInformation("Lấy danh sách bài viết từ CSDL");
      var posts = await _blogRepository
        .GetPagedPostsAsync(
        query: postQuery,
        pageNumber: pageNumber,
        pageSize: pageSize
        );
      if (pageNumber > posts.PageCount)
      {
        pageNumber = pageNumber - 1;
        ViewBag.Items = await _blogRepository
          .GetPagedPostsAsync(
          query: postQuery,
          pageNumber: pageNumber,
          pageSize: pageSize
          );
      }
      else
      {
        ViewBag.Items = posts;
      }

      ViewBag.PostQuery = postQuery;
      ViewBag.ActionName = RouteData.Values["action"];
      ViewBag.ControllerName = RouteData.Values["controller"];

      _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");
      await PopulatePostFilterModelAsync(model);

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id = 0)
    {
      Post post = id > 0
        ? await _blogRepository.FindPostByIdAsync(id, true)
        : null;

      var model = post == null
        ? new PostEditModel()
        : _mapper.Map<PostEditModel>(post);
      await PopulatePosEditModelAsync(model);

      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    [FromServices] IValidator<PostEditModel> postValidator,
      PostEditModel model)
    {
      var validationResult = await postValidator.ValidateAsync(model);

      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
      }

      if (!ModelState.IsValid)
      {
        await PopulatePosEditModelAsync(model);
        return View(model);
      }
      var post = model.Id > 0
        ? await _blogRepository.FindPostByIdAsync(model.Id)
        : null;

      if (post == null)
      {
        post = _mapper.Map<Post>(model);
        post.Id = 0;
        post.PostedDate = DateTime.Now;
      }
      else
      {
        _mapper.Map(model, post);
        post.Category = null;
        post.ModifiedDate = DateTime.Now;
      }

      if (model.ImageFile?.Length > 0)
      {
        var newImagePath = await _mediaManager.SaveFileAsync(
          model.ImageFile.OpenReadStream(),
          model.ImageFile.FileName,
          model.ImageFile.ContentType);

        if (!string.IsNullOrWhiteSpace(newImagePath))
        {
          await _mediaManager.DeleteFileAsync(post.ImageUrl);
          post.ImageUrl = newImagePath;
        }
      }

      await _blogRepository.AddOrUpdatePostAsync(
        post, model.GetSelectedTags());

      return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> VerifyPostSlug(
      int id, string urlSlug)
    {
      var slugExisted = await _blogRepository
        .IsPostSlugExistedAsync(id, urlSlug);

      return slugExisted
        ? Json($"Slug '{urlSlug}' đã được sử dụng")
        : Json(true);
    }

    [HttpGet]
    public async Task<IActionResult> ToggleStatus(
      int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize
      )
    {
      await _blogRepository.ChangePostPusblishedStateAsync(id);

      return Redirect($"{Url.ActionLink("Index",
        "Posts", new { p = pageNumber, ps = pageSize })}{queryFilter}");
    }


    [HttpGet]
    public async Task<IActionResult> Delete(
      int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize
      )
    {
      await _blogRepository.DeletePostByIdAsync(id);

      return Redirect($"{Url.ActionLink("Index",
            "Posts", new { p = pageNumber, ps = pageSize })}{queryFilter}");

    }


    private async Task PopulatePostFilterModelAsync(PostFilterModel model)
    {
      var authors = await _authorRepository.GetAllAuthorsAsync();
      var categories = await _blogRepository.GetCategoriesAsync();

      model.Authors = authors.Select(a => new SelectListItem()
      {
        Text = a.FullName,
        Value = a.Id.ToString(),
      });

      model.Categories = categories.Select(c => new SelectListItem()
      {
        Text = c.Name,
        Value = c.Id.ToString(),
      });
    }


    private async Task PopulatePosEditModelAsync(PostEditModel model)
    {
      var authors = await _authorRepository.GetAllAuthorsAsync();
      var categories = await _blogRepository.GetCategoriesAsync();

      model.Authors = authors.Select(a => new SelectListItem()
      {
        Text = a.FullName,
        Value = a.Id.ToString(),
      });

      model.Categories = categories.Select(c => new SelectListItem()
      {
        Text = c.Name,
        Value = c.Id.ToString(),
      });
    }
  }
}
