using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.WinApp
{
  public class PostItem
  {
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string UrlSlug { get; set; }
    public string ViewCount { get; set; }
    public string Tags { get; set; }

    public override string ToString()
    {
      return String.Format("{0,-5}{1,-30}{2,-50}{3,-30}{4,-10}",
        Id, Title, ShortDescription, UrlSlug, ViewCount
      );
    }
  }
}
