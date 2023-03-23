using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using TatBlog.WebApp.Extensions;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class AuthorFilterModel
  {
    [DisplayName("Từ khóa ...")]
    public string KeyWord { get; set; }

    [DisplayName("Năm tham gia ...")]
    public int? JoinedYear { get; set; }

    [DisplayName("Tháng tham gia ...")]
    public int? JoinedMonth { get; set; }

    public IEnumerable<SelectListItem> Months { get; set; }

    public AuthorFilterModel()
    {
      Months = Months.GetMonthsNameSelectsItem();
    }
  }
}
