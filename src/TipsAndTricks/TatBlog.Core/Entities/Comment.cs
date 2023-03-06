using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
  public class Comment
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public string Feedback { get; set; }
    public string NameUserComment { get; set; }
    public string Email { get; set; }
    public DateTime CommentedDate { get; set; }
    public bool? IsApproved { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }

    public override string ToString()
    {
      return String.Format("{0, -5}{1, -30}{2,-20}{3,-30}{4,-30}{5,-10}{6,-1}",
        Id, Content, NameUserComment, Email, CommentedDate, IsApproved, PostId);
    }
  }
}
