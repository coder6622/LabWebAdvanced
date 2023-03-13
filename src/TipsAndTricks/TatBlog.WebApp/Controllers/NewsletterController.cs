using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
  public class NewsletterController : Controller
  {
    private readonly ISubscriberRepository _subscriberRepository;

    public NewsletterController(ISubscriberRepository subscriberRepository)
    {
      _subscriberRepository = subscriberRepository;
    }

    public async Task<IActionResult> Subscribe(string email)
    {
      var subcriber = await _subscriberRepository.SubscribeAsync(email);
      return View();
    }
    public async Task<IActionResult> Unsubscribe(string email)
    {
      return View();
    }
  }
}
