using System.ComponentModel;

namespace TatBlog.WebApp.Models
{
  public class CommentEditModel
  {
    public int Id { get; set; }

    [DisplayName("Nội dung")]
    public string Content { get; set; }

    [DisplayName("Họ và tên")]
    public string NameUserComment { get; set; }

    [DisplayName("Email")]
    public string Email { get; set; }

    public int PostId { get; set; }
  }
}
