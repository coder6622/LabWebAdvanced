using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
  public class CommentQuery
  {
    public string Keyword { get; set; }
    public int PostId { get; set; }
    public int CommentedMonth { get; set; }
    public int CommentedYear { get; set; }
    public bool NotApprovedOnly { get; set; }
  }
}
