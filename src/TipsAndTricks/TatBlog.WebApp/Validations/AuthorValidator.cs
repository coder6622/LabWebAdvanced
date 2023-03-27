using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
  public class AuthorValidator : AbstractValidator<AuthorEditModel>
  {

    private readonly IAuthorRepository _authorRepository;
    public AuthorValidator(IAuthorRepository authorRepository)
    {
      _authorRepository = authorRepository;

      RuleFor(x => x.FullName)
        .NotEmpty()
        .WithMessage("Họ tên tác giả không được để trống")
        .MaximumLength(200)
        .WithMessage("Độ dài tên nhỏ hơn 200 ký tự");

      RuleFor(x => x.UrlSlug)
        .MustAsync(async (authModel, slug, cancellationToken) =>
          !await authorRepository
          .IsAuthorSlugExist(authModel.Id, slug, cancellationToken)
          )
        .WithMessage(x => $"Slug '{x.UrlSlug} đã được sử dụng'");

      When(x => x.Id <= 0, () =>
      {
        RuleFor(x => x.ImageFile)
        .Must(i => i is { Length: > 0 })
        .WithMessage("Bạn phải chọn hình ảnh đại diện ");
      })
    .Otherwise(() =>
    {
      RuleFor(x => x.ImageFile)
      .MustAsync(SetImageIfNotExist)
      .WithMessage("Bạn phải chọn hình ảnh đại diện");
    });

      //public string Email { get; set; }
      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage("Email không được để trống")
        .EmailAddress()
        .WithMessage("Email không đúng");

      //public string Notes { get; set; }
      RuleFor(x => x.Notes)
        .NotEmpty()
        .WithMessage("Ghi chú không được để trống");
    }

    private async Task<bool> SetImageIfNotExist(
    AuthorEditModel authorModel,
    IFormFile imageFile,
    CancellationToken cancellationToken)
    {
      var post = await _authorRepository.GetAuthorByIdAsync(
        authorModel.Id, false, cancellationToken);

      if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
        return true;

      return imageFile is { Length: > 0 };
    }
  }
}
