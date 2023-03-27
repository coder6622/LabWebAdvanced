using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
  public class CommentItem
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public string Feedback { get; set; }
    public string NameUserComment { get; set; }
    public string Email { get; set; }
    public DateTime CommentedDate { get; set; }
    public bool IsApproved { get; set; }
    public string TitlePost { get; set; }
    public string NameAuthorPost { get; set; }
  }
}
