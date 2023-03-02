using Microsoft.EntityFrameworkCore.Storage;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Tags;
using TatBlog.WinApp;

var context = new BlogDbContext();
var seeder = new DataSeeder(context);

seeder.Initialize();

#region section 4 
//var authors = context.Authors.ToList();

//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");

//foreach (var author in authors)
//{
//  Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/DatabaseDependencies/yyyy}",
//    author.Id, author.FullName, author.Email, author.JoinedDate);
//}
#endregion


#region section 5 
//var posts = context.Posts
//  .Where(p => p.Published)
//  .OrderBy(p => p.Title)
//  .Select(p => new
//  {
//    Id = p.Id,
//    Title = p.Title,
//    ViewCount = p.ViewCount,
//    PostedDate = p.PostedDate,
//    Author = p.Author.FullName,
//    Category = p.Category.Name,
//  }).ToList();
#endregion

#region section 6 
IBlogRepository blogRepo = new BlogRepository(context);
//var posts = await blogRepo.GetPopularArticlesAsync(3);
#endregion

//foreach (var post in posts)
//{
//  Console.WriteLine("Id      : {0}", post.Id);
//  Console.WriteLine("Title   : {0}", post.Title);
//  Console.WriteLine("View    : {0}", post.ViewCount);
//  Console.WriteLine("Date    : {0:MM/dd/yyyy}", post.PostedDate);
//  Console.WriteLine("Author  : {0}", post.Author);
//  Console.WriteLine("Category: {0}", post.Category);
//  Console.WriteLine("".PadRight(80, '-'));
//}

#region section 7
//var categories = await blogRepo.GetCategoriesAsync();

//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

//foreach (var category in categories)
//{
//  Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    category.Id, category.Name, category.PostCount);
//}
#endregion

#region section 8
var pagingParams = new PagingParams()
{
  PageNumber = 1,
  PageSize = 5,
  SortColumn = "Name",
  SortOrder = "DESC"
};

//var tags = await blogRepo.GetPagedTagsAsync(pagingParams);
//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//  "ID", "Name", "Count");

//foreach (var tag in tags)
//{
//  Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    tag.Id, tag.Name, tag.PostCount);
//}
#endregion


ITagRepository tagRepository = new TagRepository(context);

//var tag = await tagRepository.GetTagBySlug("google-apps");
//Console.WriteLine("{0,-5}{1,-10}{2,-20}{3,-40}{4,-5}",
//    tag.Id, tag.Name, tag.UrlSlug, tag.Description, tag.Posts.Count(
//    p => p.Published));
//foreach (var post in tag.Posts)
//{
//    Console.WriteLine(post.UrlSlug);
//}

await tagRepository.RemoveTagById(4);
