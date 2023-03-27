using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class AuthorEditModel
  {
    public int Id { get; set; }

    [DisplayName("Họ tên")]
    public string FullName { get; set; }

    [DisplayName("Slug")]
    public string UrlSlug { get; set; }

    [DisplayName("Hình đại diện")]
    public string ImageUrl { get; set; }

    [DisplayName("Địa chỉ email")]
    public string Email { get; set; }

    [DisplayName("Ghi chú")]
    public string Notes { get; set; }

    [DisplayName("Chọn hình đại diện")]
    public IFormFile ImageFile { get; set; }
  }
}
