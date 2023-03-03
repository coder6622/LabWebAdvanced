using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
  public class CategoryItem
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlSlug { get; set; }
    public string Description { get; set; }
    public bool ShowOnMenu { get; set; }
    public int PostCount { get; set; }

    public override string ToString()
    {
      return String.Format("{0,-5}{1,-20}{2,-20}{3, -40}{4,-8}{5, -10}", Id, Name, UrlSlug, Description, ShowOnMenu, PostCount);
    }
  }
}
