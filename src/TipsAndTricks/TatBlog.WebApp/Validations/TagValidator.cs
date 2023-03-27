using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
  public class TagValidator : AbstractValidator<TagEditModel>
  {
    private readonly IBlogRepository _blogRepository;

    public TagValidator(IBlogRepository blogRepository)
    {
      _blogRepository = blogRepository;

      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Tên thẻ không được để trống");


      RuleFor(x => x.UrlSlug)
        .MustAsync(async (tagModel, slug, cancellationToken) =>
          !await _blogRepository
          .IsTagSlugExistAsync(tagModel.Id, slug, cancellationToken))
        .WithMessage(x => $"Slug '{x.UrlSlug}' đã tồn tại");

      RuleFor(x => x.Description)
        .NotEmpty()
        .WithMessage("Mô tả không được để trống");
    }
  }
}
