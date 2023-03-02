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

    public async Task<IList<TagItem>> GetAllTags(CancellationToken cancellationToken = default)
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

    public async Task<Tag> GetTagBySlug(string slug, CancellationToken cancellationToken = default)
    {
      IQueryable<Tag> tagsQuery = _context.Set<Tag>()
        .Include(t => t.Posts);

      if (!string.IsNullOrEmpty(slug))
      {
        tagsQuery = tagsQuery.Where(t => t.UrlSlug == slug);
      }

      return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> RemoveTagById(int id, CancellationToken cancellationToken = default)
    {
      //await _context.Set<Tag>().Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken);




      //if (tag == null)
      //{
      //  return false;
      //}
      //_context.Set<Tag>().Remove(tag);
      //await _context.SaveChangesAsync(cancellationToken);
      //return true;

      //await _context.Set<Post>().Include(p => p.Tags).Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken);

      //await _context.Set<Tag>().Include(t => t.Posts).Where(p => p.Posts.)).ExecuteDeleteAsync(cancellationToken);


      //var targetToDelete =  await _context.Set<Tag>().Include(t => t.Posts)
      //   .Where(t => t.Id == id)
      //   .FirstOrDefaultAsync(cancellationToken);

      // _context.Set<Tag>().Remove(targetToDelete);

      //var post = await _context.Set<Post>()
      //           .Include(p => p.Tags)
      //           .SingleOrDefaultAsync(p => p.Id.Equals(id));

      //var tagDelete = await _context.Set<Tag>().FindAsync(id);


      //post.Tags.Remove(tagDelete);
      //await _context.SaveChangesAsync(cancellationToken);

      var tagToDelete = await _context.Set<Tag>()
         .Include(t => t.Posts)
         .Where(t => t.Id == id)
         .FirstOrDefaultAsync(cancellationToken);

      _context.Set<Tag>().Remove(tagToDelete);
      await _context.SaveChangesAsync(cancellationToken);

      return true;

    }

  }
}
