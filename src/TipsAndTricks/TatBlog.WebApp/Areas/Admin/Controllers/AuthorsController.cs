﻿using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    public AuthorsController(
      IAuthorRepository authorRepository,
      IMapper mapper)
    {
      _authorRepository = authorRepository;
      _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> Index(
      AuthorFilterModel model,
      [FromQuery(Name = "p")] int pageNumber = 1,
      [FromQuery(Name = "ps")] int pageSize = 2)
    {
      var authorQuery = _mapper.Map<AuthorQuery>(model);

      var authors = await _authorRepository
        .GetPagedAuthorAsync(
          authorQuery, pageNumber, pageSize);

      //if (pageNumber > authors.PageCount)
      //{
      //  authors = await _authorRepository
      //    .GetPagedAuthorAsync(
      //      authorQuery,
      //      pageNumber: pageNumber - 1,
      //      pageSize: pageSize);
      //}


      ViewBag.Authors = authors;
      ViewBag.AuthorQuery = authorQuery;

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id = 0)
    {
      Author author = id > 0
     ? await _authorRepository.FindAuthorByIdAsync(id, true)
     : null;

      var model = author == null
        ? new AuthorEditModel()
        : _mapper.Map<AuthorEditModel>(author);
      //await PopulatePosEditModelAsync(model);

      return View(model);
    }
  }
}
