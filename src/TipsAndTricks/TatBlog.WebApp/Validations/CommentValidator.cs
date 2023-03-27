using FluentValidation;
using TatBlog.WebApp.Models;

namespace TatBlog.WebApp.Validations
{
  public class CommentValidator : AbstractValidator<CommentEditModel>
  {

    public CommentValidator()
    {
      RuleFor(c => c.Email)
        .NotEmpty()
        .WithMessage("Email không được để trống")
        .EmailAddress()
        .WithMessage("Email không hợp lệ");

      RuleFor(c => c.Content)
        .NotEmpty()
        .WithMessage("Nội dung không được để trống");
    }
  }
}
