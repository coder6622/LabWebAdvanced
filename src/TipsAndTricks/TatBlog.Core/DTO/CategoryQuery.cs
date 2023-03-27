using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
  public class CategoryQuery
  {
    public string KeyWord { get; set; }
    public bool NotShowOnMenu { get; set; }
  }
}
