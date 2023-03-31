using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Models.Comment
{
  public class CommentDto
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public string Feedback { get; set; }
    public string NameUserComment { get; set; }
    public string Email { get; set; }
    public DateTime CommentedDate { get; set; }
    public bool? IsApproved { get; set; }
    public int PostId { get; set; }
  }
}
