using FluentValidation;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Validations
{
  public class TagValidator : AbstractValidator<TagEditModel>
  {
    public TagValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Tên thẻ không được để trống");

      RuleFor(x => x.UrlSlug)
        .NotEmpty()
        .WithMessage("Slug không được để trống");

      RuleFor(x => x.Description)
        .NotEmpty()
        .WithMessage("Mô tả không được để trống");
    }

  }
}
