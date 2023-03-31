using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
  public class StatisticalItem
  {
    public int PostCount { get; set; }
    public int PostCountNotPushlished { get; set; }
    public int CategoryCount { get; set; }
    public int AuthorCount { get; set; }
    public int CommentCount { get; set; }
    public int CommentWaitingAproveCount { get; set; }
  }
}
