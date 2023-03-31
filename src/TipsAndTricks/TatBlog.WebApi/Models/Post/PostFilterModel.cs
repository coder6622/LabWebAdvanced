namespace TatBlog.WebApi.Models.Post
{
  public class PostFilterModel : PagingModel
  {
    public string Keyword { get; set; }

    public int? AuthorId { get; set; }

    public int? CategoryId { get; set; }

    public bool? NotPublished { get; set; }

    public int? PostedYear { get; set; }

    public int? PostedMonth { get; set; }
  }
}
