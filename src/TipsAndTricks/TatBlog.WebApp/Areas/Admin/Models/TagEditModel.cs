using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class TagEditModel
  {
    public int Id { get; set; }

    [DisplayName("Tên thẻ")]
    public string Name { get; set; }

    [DisplayName("Slug")]
    public string UrlSlug { get; set; }

    [DisplayName("Mô tả thẻ")]
    public string Description { get; set; }
  }
}
