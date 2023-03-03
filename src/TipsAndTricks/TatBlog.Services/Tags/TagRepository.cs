using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Tags
{
  public class TagRepository : ITagRepository
  {

    private readonly BlogDbContext _context;
    public TagRepository(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
      IQueryable<Tag> tags = _context.Set<Tag>();
      return await tags
        .OrderBy(t => t.Name)
        .Select(t => new TagItem()
        {
          Id = t.Id,
          Name = t.Name,
          UrlSlug = t.UrlSlug,
          Description = t.Description,
          PostCount = t.Posts.Count(p => p.Published)
        })
        .ToListAsync(cancellationToken);
      ;
    }

    public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
      IQueryable<Tag> tagsQuery = _context.Set<Tag>()
        .Include(t => t.Posts);

      if (!string.IsNullOrEmpty(slug))
      {
        tagsQuery = tagsQuery.Where(t => t.UrlSlug == slug);
      }

      return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> RemoveTagByIdAsync(int id, CancellationToken cancellationToken = default)
    {
      // find -> delete 
      var tagToDelete = await _context.Set<Tag>()
         .Include(t => t.Posts)
         .Where(t => t.Id == id)
         .FirstOrDefaultAsync(cancellationToken);
      if (tagToDelete == null)
      {
        return false;
      }

      _context.Set<Tag>().Remove(tagToDelete);
      await _context.SaveChangesAsync(cancellationToken);


      // dung cau lenh sql 
      //await _context.Database
      //       .ExecuteSqlRawAsync($"DELETE FROM PostTags WHERE TagsId={id}", cancellationToken);

      //await _context.Set<Tag>()
      //    .Where(t => t.Id == id)
      //    .ExecuteDeleteAsync(cancellationToken);

      return true;
    }

  }
}
