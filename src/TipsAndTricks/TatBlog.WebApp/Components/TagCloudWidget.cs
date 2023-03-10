using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
  public class TagCloudWidget : ViewComponent
  {
    IBlogRepository _blogRepository;

    public TagCloudWidget(IBlogRepository blogRepository)
    {
      _blogRepository = blogRepository;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

      var tags = await _blogRepository.GetAllTagsAsync();

      return View(tags);
    }

  }
}
