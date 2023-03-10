using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
  public class FeaturedPostsWidget : ViewComponent
  {
    IBlogRepository _blogRepository;

    public FeaturedPostsWidget(IBlogRepository blogRepository)
    {
      _blogRepository = blogRepository;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

      var top3PostsMostView = await _blogRepository
        .GetPopularArticlesAsync(3);

      return View(top3PostsMostView);
    }

  }
}
