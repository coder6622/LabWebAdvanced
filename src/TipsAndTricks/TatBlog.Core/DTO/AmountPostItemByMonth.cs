using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
  public class AmountPostItemByMonth
  {
    public int Year { get; set; }
    public int Month { get; set; }
    public int PostCount { get; set; }

    public string MonthName
    {
      get => CultureInfo
        .CurrentCulture
        .DateTimeFormat
        .GetMonthName(Month);
    }

    public override string ToString()
    {
      return String.Format(" {0,-10}{1, -10}{2,-10}", Year, Month, PostCount);
    }
  }
}
