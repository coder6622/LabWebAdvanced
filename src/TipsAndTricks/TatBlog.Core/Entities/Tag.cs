using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
  public class Tag : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlSlug { get; set; }
    public string Description { get; set; }

    public IList<Post> Posts { get; set; }

    public override string ToString()
    {
      return String.Format("{0, -10}{1,-20}{2,-30}{3,-60}", Id, Name, UrlSlug, Description);
    }
  }
}
