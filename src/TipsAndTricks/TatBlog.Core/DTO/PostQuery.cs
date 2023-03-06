using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
  public class PostQuery
  {
    public string Keyword { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int AuthorId { get; set; }
    public int PostedMonth { get; set; }
    public string SelectedTags { get; set; }

    public List<string> GetSelectedTags()
    {
      return (SelectedTags ?? "")
        .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        .ToList();
    }
  }
}
