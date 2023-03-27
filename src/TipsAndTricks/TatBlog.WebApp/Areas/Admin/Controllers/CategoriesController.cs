using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;
    public CategoriesController(
      IBlogRepository blogRepository,
      IMapper mapper)
    {
      _blogRepository = blogRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
      CategoryFilterModel model,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 2)
    {
      var categoryQuery = _mapper.Map<CategoryQuery>(model);

      var categories = await _blogRepository
        .GetPagedCategoriesAsync<CategoryItem>(
        categoryQuery,
        pageNumber,
        pageSize,
        category => category.ProjectToType<CategoryItem>());

      if (pageNumber > categories.PageCount)
      {
        categories = await _blogRepository
          .GetPagedCategoriesAsync<CategoryItem>(
          categoryQuery,
          pageNumber - 1,
          pageSize,
          category => category.ProjectToType<CategoryItem>());
      }

      ViewBag.Items = categories;

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ToggleShowOnMenu(
       int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize)
    {
      await _blogRepository
        .ChangeCategoriesShowOnMenu(id);

      return Redirect($"{Url
        .ActionLink(
          "Index",
          "Categories",
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
      await _blogRepository.DeleteCategoryByIdAsync(id);

      return Redirect($"{Url
        .ActionLink(
            "Index",
            "Categories",
            new { p = pageNumber, ps = pageSize })}{queryFilter}");

    }

    [HttpGet]
    public async Task<IActionResult> Edit(
      int id = 0)
    {
      Category category = id > 0
        ? await _blogRepository
        .FindCategoryByIdAsync(id)
        : null;

      var model = category == null
        ? new CategoryEditModel()
        : _mapper.Map<CategoryEditModel>(category);


      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
      [FromServices] IValidator<CategoryEditModel> categoryValidator,
      CategoryEditModel model
      )
    {
      var validationResult = await categoryValidator
        .ValidateAsync(model);

      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
      }

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var category = model.Id > 0
        ? await _blogRepository.FindCategoryByIdAsync(model.Id)
        : null;

      if (category == null)
      {
        category = _mapper.Map<Category>(model);
        category.Id = 0;
      }
      else
      {
        _mapper.Map(model, category);
      }

      await _blogRepository.AddOrUpdateCategoryAsync(category);

      return RedirectToAction(nameof(Index));
    }

  }
}
