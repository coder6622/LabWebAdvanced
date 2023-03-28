using FluentValidation;
using TatBlog.WebApi.Models.Category;

namespace TatBlog.WebApi.Validations
{
  public class CategoryValidator : AbstractValidator<CategoryEditModel>
  {

    public CategoryValidator()
    {
      RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Tên chủ đề không được để trống")
      .MaximumLength(200)
      .WithMessage("Độ dài tên phải lớn hơn 200 ký tự");

      RuleFor(x => x.UrlSlug)
          .NotEmpty()
          .WithMessage("UrlSlug không được để trống")
          .MaximumLength(100)
          .WithMessage("UrlSlug tối đa 100 ký tự");

      RuleFor(x => x.Description)
        .NotEmpty()
        .WithMessage("Mô tả không được để trống")
        .MaximumLength(5000)
        .WithMessage("Mô tả không lớn hơn 5000 ký tự");
    }

  }
}
