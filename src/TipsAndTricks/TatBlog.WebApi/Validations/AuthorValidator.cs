using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.Author;

namespace TatBlog.WebApi.Validations
{
    public class AuthorValidator : AbstractValidator<AuthorEditModel>
  {
    public AuthorValidator()
    {
      RuleFor(x => x.FullName)
        .NotEmpty()
        .WithMessage("Họ tên tác giả không được để trống")
        .MaximumLength(100)
        .WithMessage("Độ dài tên nhỏ hơn 100 ký tự");

      RuleFor(x => x.UrlSlug)
        .NotEmpty()
        .WithMessage("UrlSlug không được để trống")
        .MaximumLength(100)
        .WithMessage("UrlSlug tối đa 100 ký tự");

      RuleFor(a => a.JoinedDate)
        .GreaterThan(DateTime.MinValue)
        .WithMessage("Ngày tham gia không hợp lệ");

      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage("Email không được để trống")
        .EmailAddress()
        .WithMessage("Email không đúng");

      RuleFor(x => x.Notes)
        .MaximumLength(500)
        .WithMessage("Ghi chú tối đa 500 ký tự");
    }
  }
}
