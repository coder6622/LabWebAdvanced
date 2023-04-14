using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Category;

namespace TatBlog.WebApi.Models.Post
{
    public class PostFilterData
    {
        public IList<AuthorDto> authors { get; set; }
        public IList<CategoryDto> categories { get; set; }
    }
}
