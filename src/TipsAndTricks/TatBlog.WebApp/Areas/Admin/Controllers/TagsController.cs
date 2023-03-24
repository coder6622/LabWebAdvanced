using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class TagsController : Controller
  {
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public TagsController(
      IBlogRepository blogRepository,
      IMapper mapper)
    {
      _blogRepository = blogRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
      TagFilterModel model,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 2)
    {
      var tagQuery = _mapper.Map<TagQuery>(model);

      var tags = await _blogRepository
        .GetPagedTagsAsync<TagItem>(
          tagQuery,
          pageNumber,
          pageSize,
          tags => tags.ProjectToType<TagItem>());
      Console.WriteLine(tags);

      ViewBag.Items = tags;

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(
      int id = 0)
    {

      Tag tag = id > 0
        ? await _blogRepository.GetTagByIdAsync(id)
        : null;

      var model = tag == null
        ? new TagEditModel()
        : _mapper.Map<TagEditModel>(tag);

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
     [FromServices] IValidator<TagEditModel> tagValidator,
       TagEditModel model)
    {

      var validationResult = await tagValidator
        .ValidateAsync(model);

      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
      }

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var tag = model.Id > 0
        ? await _blogRepository.GetTagByIdAsync(model.Id)
        : null;

      if (tag == null)
      {
        tag = _mapper.Map<Tag>(model);
        tag.Id = 0;
      }
      else
      {
        _mapper.Map(model, tag);
      }

      await _blogRepository.AddOrUpdateTagAsync(tag);

      return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Delete(
      int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize)
    {
      await _blogRepository.RemoveTagByIdAsync(id);
      return Redirect($"{Url
        .ActionLink(
            "Index",
            "Tags",
            new { p = pageNumber, ps = pageSize })}{queryFilter}");
    }
  }
}
