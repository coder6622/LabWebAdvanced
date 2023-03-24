using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class CategoryFilterModel
  {
    [DisplayName("Từ khóa")]
    public string KeyWord { get; set; }

    [DisplayName("Không hiển thị")]
    public bool NotShowOnMenu { get; set; }
  }
}
