using Microsoft.EntityFrameworkCore.Storage;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Categories;
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


// ================= Section C ===============
#region Tag test
ITagRepository tagRepository = new TagRepository(context);

//var tag = await tagRepository.GetTagBySlug("google-apps");
//Console.WriteLine("{0,-5}{1,-10}{2,-20}{3,-40}{4,-5}",
//    tag.Id, tag.Name, tag.UrlSlug, tag.Description, tag.Posts.Count(
//    p => p.Published));
//foreach (var post in tag.Posts)
//{
//    Console.WriteLine(post.UrlSlug);
//}
#endregion

//await tagRepository.RemoveTagByIdAsync(4);



#region category test
ICategoriesRepository categoriesRepository = new CategoriesRepository(context);
//var categoryById = await categoriesRepository.FindCategoryByIdAsync(1);
//Console.WriteLine(categoryById);

//Console.WriteLine(await categoriesRepository.FindCategoryBySlugAsync("architecture"));


//Console.WriteLine(await categoriesRepository.IsCategoryExistBySlugAsync( "architectures"));
//Console.WriteLine(await categoriesRepository.IsCategoryExistBySlugAsync( "architecture"));

//Console.WriteLine(await categoriesRepository.DeleteCategoryByIdAsync(1));


//Category categoryAdd = new()
//{
//  Name = "Do an co so",
//  Description = "Do an co so ne nha",
//  UrlSlug = "do-an-co-so"
//};

//Category categoryUpdate = new()
//{
//  Id = 4,
//  UrlSlug = "oop",
//  Name = "OOP",
//  Description = "Object Orient Programming",
//};

//await categoriesRepository.AddOrUpdateCategoryAsync(categoryUpdate);





var pagingParamsCategories = new PagingParams()
{
  PageNumber = 2,
  PageSize = 5,
  SortColumn = "UrlSlug",
  SortOrder = "ASC"
};

//var categories = await categoriesRepository.GetPagedCategoriesAsync(pagingParamsCategories);

//foreach (var category in categories)
//{
//  Console.WriteLine(category);
//}
#endregion

#region post test
//var amountOfPosts = await blogRepo.CountPostsInNMonthsAsync(4);
//foreach (var item in amountOfPosts)
//{
//  Console.WriteLine(item);
//}


//var postFindById = await blogRepo.FindPostByIdAsync(1);
//Console.WriteLine("{0,-5}{1,-40}{2,-50}{3,-20}",
//  postFindById.Id,
//  postFindById.Title,
//  postFindById.ShortDescription,
//  postFindById.UrlSlug
//);

//Post postAddOrUpdate =
//        new Post()
//        {
//          Id = 11,
//          Title = "ASP .NET CORE Reactjs9",
//          ShortDescription = "Duat and friends has a great repository",
//          Description = "This's bad bad bad day",
//          Meta = "Duat and friends has a greate repository filled",
//          UrlSlug = "aspnet-core-reactj9",
//          Published = true,
//          PostedDate = new DateTime(2022, 5, 25, 10, 20, 0),
//          ModifiedDate = null,
//          AuthorId = context.Authors.ToList()[1].Id,
//          CategoryId = context.Categories.ToList()[0].Id,
//          Tags = new List<Tag>()
//          {

//            context.Tags.ToList()[0],
//            context.Tags.ToList()[2],
//            context.Tags.ToList()[5],
//          },
//          ViewCount = 200,
//        };

//await blogRepo.AddOrUpdatePostAsync(postAddOrUpdate);
//await blogRepo.ChangePostPusblishedStateAsync(4, false);

//var nPostsRandom = await blogRepo.GetRandomNPosts(5);

//foreach (var post in nPostsRandom)
//{
//  Console.WriteLine(post);
//}
#endregion