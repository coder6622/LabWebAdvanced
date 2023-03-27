using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
  public class CategoryValidator : AbstractValidator<CategoryEditModel>
  {
    private readonly IBlogRepository _blogRepository;

    public CategoryValidator(IBlogRepository blogRepository)
    {
      _blogRepository = blogRepository;

      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Tên không được để trống");

      RuleFor(x => x.UrlSlug)
        .MustAsync(async (cateModel, slug, cancellationToken) => !await _blogRepository.IsCategorySlugExistAsync(cateModel.Id, slug, cancellationToken))
        .WithMessage(x => $"Slug '{x.UrlSlug}' đã tồn tại");

      RuleFor(x => x.Description)
        .NotEmpty()
        .WithMessage("Mô tả không được để trống");

    }
  }
}
