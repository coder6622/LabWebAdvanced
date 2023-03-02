using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Tags
{
  public interface ITagRepository
  {
    Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default);

    Task<bool> RemoveTagByIdAsync(int id, CancellationToken cancellationToken = default);
  }
}
