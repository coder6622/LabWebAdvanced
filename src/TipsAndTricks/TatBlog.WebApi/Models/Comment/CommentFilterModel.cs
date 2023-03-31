namespace TatBlog.WebApi.Models.Comment
{
  public class CommentFilterModel : PagingModel
  {
    public string Keyword { get; set; }
    public int? PostId { get; set; }
    public int? CommentedMonth { get; set; }
    public int? CommentedYear { get; set; }
    public bool? NotApprovedOnly { get; set; }

  }
}
