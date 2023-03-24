using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class CategoryEditModel
  {
    public int Id { get; set; }

    [DisplayName("Tên chủ đề")]
    public string Name { get; set; }

    [DisplayName("Slug")]
    public string UrlSlug { get; set; }

    [DisplayName("Mô tả")]
    public string Description { get; set; }

    [DisplayName("Hiển thị")]
    public bool ShowOnMenu { get; set; }
  }
}
