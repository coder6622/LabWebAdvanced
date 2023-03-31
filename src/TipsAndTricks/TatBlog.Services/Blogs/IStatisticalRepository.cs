using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;

namespace TatBlog.Services.Blogs
{
  public interface IStatisticalRepository
  {
    Task<StatisticalItem> GetStatisticals(CancellationToken cancellationToken = default);
  }
}
