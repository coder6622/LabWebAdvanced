using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
  public class PostEditModel
  {
    public int Id { get; set; }

    //[Required(ErrorMessage = "Tiêu đề không được để trống")]
    //[MaxLength(500, ErrorMessage = "Tiêu đề tối đa 500 ký tự")]
    [DisplayName("Tiêu đề")]
    public string Title { get; set; }

    //[Required(ErrorMessage = "Giới thiệu không được để trống")]
    //[MaxLength(2000, ErrorMessage = "Giới thiệu tối đa 2000 ký tự")]
    [DisplayName("Giới thiệu")]
    public string ShortDescription { get; set; }

    //[Required(ErrorMessage = "Nội dung không được để trống")]
    //[MaxLength(5000, ErrorMessage = "Nội dung tối đa 5000 ký tự")]
    [DisplayName("Nội dung")]
    public string Description { get; set; }

    //[Required(ErrorMessage = "Metadata không được để trống")]
    //[MaxLength(1000, ErrorMessage = "Metadata tối đa 1000 ký tự")]
    [DisplayName("Metadata")]
    public string Meta { get; set; }


    //[Remote("VerifyPostSlug", "Posts", "Admin",
    //  HttpMethod = "POST", AdditionalFields = "Id")]
    //[Required(ErrorMessage = "Url slug không được để trống")]
    //[MaxLength(200, ErrorMessage = "Url slug tối đa 200 ký tự")]
    [DisplayName("Slug")]
    public string UrlSlug { get; set; }

    [DisplayName("Chọn hình ảnh")]
    public IFormFile ImageFile { get; set; }

    [DisplayName("Hình hiện tại")]
    public string ImageUrl { get; set; }


    [DisplayName("Xuất bản ngay")]
    public bool Published { get; set; }

    //[Required(ErrorMessage = "Bạn chưa chọn chủ đề")]
    [DisplayName("Chủ đề")]
    public int CategoryId { get; set; }

    //[Required(ErrorMessage = "Bạn chưa chọn tác giả")]
    [DisplayName("Tác giả")]
    public int AuthorId { get; set; }

    //[Required(ErrorMessage = "Bạn chưa nhập tên thẻ")]
    [DisplayName("Từ khóa (mỗi từ 1 dòng)")]
    public string SelectedTags { get; set; }


    public IEnumerable<SelectListItem> Authors { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; }

    public List<string> GetSelectedTags()
    {
      return (SelectedTags ?? "")
        .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        .ToList();
    }
  }
}
