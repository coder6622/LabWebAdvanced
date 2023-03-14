using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class PostsController : Controller
  {
    private readonly IBlogRepository _blogRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    public PostsController(
      IBlogRepository blogRepository,
      IAuthorRepository authorRepository,
      IMapper mapper)
    {
      _blogRepository = blogRepository;
      _authorRepository = authorRepository;
      _mapper = mapper;
    }
    public async Task<IActionResult> Index(PostFilterModel model)
    {

      var postQuery = _mapper.Map<PostQuery>(model);
      await PopulatePostFilterModelAsync(model);

      ViewBag.Posts = await _blogRepository
        .GetPagedPostsAsync(query: postQuery, pageNumber: 1, pageSize: 10);

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id = 0)
    {
      Post post = id > 0
        ? await _blogRepository.FindPostByIdAsync(id)
        : null;

      PostEditModel model = new PostEditModel();
      if (post != null)
      {
        model = _mapper.Map<PostEditModel>(post);
      }
      //var model = post == null
      //  ? new PostEditModel()
      //  :;
      await PopulatePosEditModelAsync(model);

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PostEditModel model)
    {
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
