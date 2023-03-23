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

      config.NewConfig<PostEditModel, Post>()
        .Ignore(dest => dest.Id)
        .Ignore(dest => dest.ImageUrl);

      config.NewConfig<Post, PostEditModel>()
        .Map(dest => dest.SelectedTags, src =>
        string.Join("\r\n", src.Tags.Select(t => t.Name)))
        .Ignore(dest => dest.Authors)
        .Ignore(dest => dest.Categories)
        .Ignore(dest => dest.ImageFile);

      config.NewConfig<Author, AuthorItem>()
        .Map(dest => dest.PostsCount, src => src.Posts.Count);

      config.NewConfig<AuthorEditModel, Author>()
      .Ignore(dest => dest.Id)
      .Ignore(dest => dest.ImageUrl);
    }
  }
}
