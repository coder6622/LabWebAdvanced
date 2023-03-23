using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class AuthorEditModel
  {
    public int Id { get; set; }

    [DisplayName("Họ tên")]
    public string FullName { get; set; }


    //[Display("Slug")]
    public string UrlSlug { get; set; }
    public string ImageUrl { get; set; }
    public DateTime JoinedDate { get; set; }
    public string Email { get; set; }
    public string Notes { get; set; }
  }
}
