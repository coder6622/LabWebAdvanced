
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using TatBlog.WebApp.Extensions;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class CommentFilterModel
  {
    [DisplayName("Từ khóa")]
    public string Keyword { get; set; }

    [DisplayName("Tháng")]
    public int? CommentedMonth { get; set; }

    [DisplayName("Năm")]
    public int? CommentedYear { get; set; }

    [DisplayName("Chưa duyệt")]
    public bool NotApprovedOnly { get; set; }

    public IEnumerable<SelectListItem> Months { get; set; }

    public CommentFilterModel()
    {
      Months = Months.PopulateMonthNames();
      NotApprovedOnly = true;
    }
  }
}
