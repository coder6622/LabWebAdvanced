using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
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
  }
}
