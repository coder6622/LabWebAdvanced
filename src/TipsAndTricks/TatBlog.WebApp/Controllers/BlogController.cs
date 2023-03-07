using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Controllers
{
  public class BlogController : Controller
  {
    public IActionResult Index()
    {
      ViewBag.CurrentTime = DateTime.Now
        .ToString("HH:mm:ss");
      return View();
    }

    public IActionResult About()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }

    public IActionResult Rss()
    {
      return Content("Nội dung sẽ được cập nhật");
    }
  }
}
