using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
  public class AmountOfPostByDate
  {
    public int Year { get; set; }
    public int Month { get; set; }
    public int PostCount { get; set; }
    public override string ToString()
    {
      return String.Format("{0,-10}{1, -10}{2,-10}", Year, Month, PostCount);
    }
  }
}
