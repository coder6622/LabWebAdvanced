using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly IAuthorRepository _authorRepository;
    private readonly IMediaManager _mediaManager;
    private readonly IMapper _mapper;
    public AuthorsController(
      IAuthorRepository authorRepository,
      IMediaManager mediaManager,
      IMapper mapper)
    {
      _authorRepository = authorRepository;
      _mapper = mapper;
      _mediaManager = mediaManager;
    }


    [HttpGet]
    public async Task<IActionResult> Index(
      AuthorFilterModel model,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 2)
    {
      var authorQuery = _mapper.Map<AuthorQuery>(model);

      var authors = await _authorRepository
        .GetPagedAuthorsAsync<AuthorItem>(
       query: authorQuery,
       pageNumber: pageNumber,
       pageSize: pageSize,
       mapper: authors =>
         authors.ProjectToType<AuthorItem>());


      if (pageNumber > authors.PageCount)
      {
        authors = await _authorRepository
           .GetPagedAuthorsAsync<AuthorItem>(
          query: authorQuery,
          pageNumber: pageNumber - 1,
          pageSize: pageSize,
          mapper: authors =>
            authors.ProjectToType<AuthorItem>());
      }


      ViewBag.Items = authors;
      ViewBag.AuthorQuery = authorQuery;

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id = 0)
    {
      Author author = id > 0
     ? await _authorRepository.GetAuthorByIdAsync(id, true)
     : null;

      var model = author == null
        ? new AuthorEditModel()
        : _mapper.Map<AuthorEditModel>(author);

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
      [FromServices] IValidator<AuthorEditModel> authorValidator,
      AuthorEditModel model
      )
    {
      var validationResult = await authorValidator.ValidateAsync(model);

      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
      }

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var author = model.Id > 0
        ? await _authorRepository.GetAuthorByIdAsync(model.Id)
        : null;

      if (author == null)
      {
        author = _mapper.Map<Author>(model);
        author.Id = 0;
        author.JoinedDate = DateTime.Now;
      }
      else
      {
        _mapper.Map(model, author);
      }

      if (model.ImageFile?.Length > 0)
      {
        var newImagePath = await _mediaManager.SaveFileAsync(
          model.ImageFile.OpenReadStream(),
          model.ImageFile.FileName,
          model.ImageFile.ContentType);

        if (!string.IsNullOrWhiteSpace(newImagePath))
        {
          await _mediaManager.DeleteFileAsync(model.ImageUrl);
          author.ImageUrl = newImagePath;
        }
      }

      await _authorRepository.AddOrUpdateAuthor(author);

      return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(
      int id,
      [FromQuery(Name = "filter")] string queryFilter,
      [FromQuery(Name = "p")] int pageNumber,
      [FromQuery(Name = "ps")] int pageSize
      )
    {
      await _authorRepository.DeleteAuthorAsync(id);

      return Redirect($"{Url.ActionLink("Index",
            "Authors", new { p = pageNumber, ps = pageSize })}{queryFilter}");

    }
  }
}
