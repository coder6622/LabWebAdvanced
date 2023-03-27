using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;
using TatBlog.Core;
using TatBlog.WebApp.Extensions;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class PostFilterModel
  {
    [DisplayName("Từ khóa")]
    public string Keyword { get; set; }

    [DisplayName("Tác giả")]
    public int? AuthorId { get; set; }

    [DisplayName("Chủ đề")]
    public int? CategoryId { get; set; }

    [DisplayName("Chưa đăng")]
    public bool NotPublished { get; set; }

    [DisplayName("Năm")]
    public int? PostedYear { get; set; }

    [DisplayName("Tháng")]
    public int? PostedMonth { get; set; }

    public IEnumerable<SelectListItem> Authors { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; }
    public IEnumerable<SelectListItem> Months { get; set; }
    public PostFilterModel()
    {
      Months = Months.PopulateMonthNames();
    }
  }
}
