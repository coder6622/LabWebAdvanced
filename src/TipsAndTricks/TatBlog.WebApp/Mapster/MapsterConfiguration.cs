using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WinApp;

namespace TatBlog.WebApp.Mapster
{
  public class MapsterConfiguration : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<Post, PostItem>()
        .Map(dest => dest.CategoryName, src => src.Category.Name)
        .Map(dest => dest.Tags, src => src.Tags.Select(t => t.Name));

      config.NewConfig<PostFilterModel, PostQuery>()
        .Map(dest => dest.PublishedOnly, src => false);
    }
  }
}
