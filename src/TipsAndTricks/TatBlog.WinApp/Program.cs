﻿using Mapster;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
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
//IBlogRepository blogRepo = new BlogRepository(context);
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
//var tagFindBySlug = await blogRepo.GetTagBySlugAsync("google-apps");
//Console.WriteLine("{0,-5}{1,-10}{2,-20}{3,-40}{4,-5}",
//    tagFindBySlug.Id, tagFindBySlug.Name, tagFindBySlug.UrlSlug, tagFindBySlug.Description,
//    tagFindBySlug.Posts.Count(p => p.Published));
//foreach (var post in tagFindBySlug.Posts)
//{
//  Console.WriteLine(post.UrlSlug);
//}
//await blogRepo.RemoveTagByIdAsync(4);
#endregion




#region category test
//var categoryFindById = await blogRepo.FindCategoryByIdAsync(1);
//Console.WriteLine(categoryFindById);

//Console.WriteLine(await blogRepo.FindCategoryBySlugAsync("architecture"));


//Console.WriteLine(await blogRepo.IsCategoryExistBySlugAsync("architectures"));
//Console.WriteLine(await blogRepo.IsCategoryExistBySlugAsync("architecture"));

//Console.WriteLine(await blogRepo.DeleteCategoryByIdAsync(8));


//Category categoryToAdd = new()
//{
//  Name = "Do an co so",
//  Description = "Do an co so",
//  UrlSlug = "do-an-co-so"
//};

//Category categoryToUpdate = new()
//{
//  Id = 4,
//  UrlSlug = "oop",
//  Name = "OOP",
//  Description = "Object Orient Programming",
//};

//await blogRepo.AddOrUpdateCategoryAsync(categoryToAdd);


//var pagingParamsCategories = new PagingParams()
//{
//  PageNumber = 2,
//  PageSize = 5,
//  SortColumn = "UrlSlug",
//  SortOrder = "ASC"
//};

//var categories = await blogRepo.GetPagedCategoriesAsync(pagingParamsCategories);

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
//          Id = 12,
//          Title = "ASP .NET CORE Reactjs12",
//          ShortDescription = "Duat and friends has a great repository",
//          Description = "This's bad bad bad day",
//          Meta = "Duat and friends has a greate repository filled",
//          UrlSlug = "aspnet-core-reactj12",
//          Published = true,
//          PostedDate = new DateTime(2022, 5, 25, 10, 20, 0),
//          ModifiedDate = null,
//          AuthorId = context.Authors.ToList()[1].Id,
//          CategoryId = context.Categories.ToList()[0].Id,
//          ViewCount = 200,
//        };


//var tagForPost = new List<Tag>()
//{
//  context.Tags.ToList()[0],
//  context.Tags.ToList()[2],
//  context.Tags.ToList()[5],
//};

//await blogRepo.AddOrUpdatePostAsync(postAddOrUpdate, tagForPost);

//await blogRepo.ChangePostPusblishedStateAsync(12, false);

//var nPostsRandom = await blogRepo.GetRandomNPosts(5);

//foreach (var post in nPostsRandom)
//{
//  Console.WriteLine(post);
//}

//=====================================================TIM POST VOI QUERY=========================================
// tim voi query
//PostQuery postQuery = new PostQuery()
//{
//  Keyword = "reactjs",
//  //PostedMonth = 6,
//  SelectedTags = "ASP .NET MVC;Razor Page",
//  //AuthorId = 1
//};

//var postFindByQuery = await blogRepo.FindPostsByQueryAsync(postQuery);

//foreach (var post in postFindByQuery)
//{
//  Console.Write(post + "||");
//  foreach (var tag in post.Tags)
//  {
//    Console.Write($"{tag.Name}|");
//  }
//  Console.WriteLine();
//}

//var countPostsFindByQuery = await blogRepo.CountPostsSatisfyQueryAsync(postQuery);
//Console.WriteLine(countPostsFindByQuery);


//var pagingParamsFindPost = new PagingParams()
//{
//  PageNumber = 1,
//  PageSize = 5,
//  SortColumn = "Title",
//  SortOrder = "ASc"
//};

//var postFindByQueryWithPaginate = await blogRepo
//  .FindAndPaginatePostByQueryAsync(postQuery, pagingParamsFindPost);

//Console.WriteLine("\n=================TIM CO PHAN TRANG=====================\n");
//foreach (var post in postFindByQueryWithPaginate)
//{
//  Console.Write(post + "||");
//  foreach (var tag in post.Tags)
//  {
//    Console.Write($"{tag.Name}|");
//  }
//  Console.WriteLine();
//}

//Console.WriteLine("\n====================ANH XA SANG POST MAPPER TEST==================\n");
//IPagedList<PostMapperTest> postMapperTestFindByQueryWithPaginate = await blogRepo
//  .FindAndPaginatePostAsync<PostMapperTest>(
//  postQuery,
//  pagingParamsFindPost,
//  posts => posts.ProjectToType<PostMapperTest>()
//  );

//foreach (var post in postMapperTestFindByQueryWithPaginate)
//{
//  Console.WriteLine(post);
//}
#endregion

#region author test
//IAuthorRepository authorRepository = new AuthorRepository(context);

//Author authorFindById = await authorRepository.FindAuthorByIdAsync(3);
//Console.WriteLine(authorFindById);

//Author authorFindBySlug = await authorRepository.FindAuthorBySlugAsync("Hoang-long");
//Console.WriteLine(authorFindBySlug);

//Author authorTestUpdateaOrAdd = new Author()
//{
//  Id = 4,
//  FullName = "Nguyen Xuan Hung",
//  UrlSlug = "xuan-hung",
//  Email = "xuanhung@gmail.com",
//  JoinedDate = new DateTime(2023, 2, 21)
//};

//await authorRepository.AddOrUpdateAuthor(authorTestUpdateaOrAdd);

//IPagingParams pagingParamsAuthor = new PagingParams()
//{
//  PageNumber = 1,
//  PageSize = 5,
//  SortColumn = "FullName",
//  SortOrder = "ASC"
//};

//var authorTopPosts = await authorRepository.GetNAuthorTopPosts(3, pagingParamsAuthor);
//foreach (var author in authorTopPosts)
//{
//  Console.WriteLine(author);
//}


//var allAuthors = await authorRepository.GetAllAuthor(pagingParamsAuthor);
//foreach (var author in allAuthors)
//{
//  Console.WriteLine(author);
//}
#endregion
#region Subscriber test
ISubscriberRepository subscriberRepository = new SubscriberRepository(context);

//var subscriberFindByEmail = await subscriberRepository
//  .GetSubscriberByEmailAsync("hoanglong@gmail.com");

//var subscriberFindById = await subscriberRepository
//  .GetSubscriberByIdAsync(5);

//Console.WriteLine(subscriberFindByEmail);
//Console.WriteLine(subscriberFindById);

var newSubscriber = await subscriberRepository.SubscribeAsync("nhattien@gmail.com");
Console.WriteLine(newSubscriber);

Console.WriteLine(await subscriberRepository
  .UnsubscribeAsync("minhtien@gmail.com", "", true));

Console.WriteLine(await subscriberRepository
  .BlockSubscriberAsync(1004, "Tui khong thich thang nay", "Thong tin gia mao"));

Console.WriteLine(await subscriberRepository.DeleteSubscriberAsync(1003));

IPagingParams pagingParamsSubscriber = new PagingParams()
{
  PageNumber = 1,
  PageSize = 5,
  SortColumn = "Email",
  SortOrder = "ASC"
};

var subscribersSearch = await subscriberRepository
  .SearchSubscribersAsync(pagingParamsSubscriber, "tien", false, true);

foreach (var subscriber in subscribersSearch)
{
  Console.WriteLine(subscriber);
}

#endregion

#region comment test

ICommentRepository commentRepository = new CommentRepository(context);

var commentFindById = await commentRepository.GetCommentByIdAsync(1);
Console.WriteLine(commentFindById);
Console.WriteLine("=========");

var commentFindByPostId = await commentRepository.GetCommentsByPostIdAsync(4);
foreach (var comment in commentFindByPostId)
{
  Console.WriteLine(comment);
}

Console.WriteLine("=========");
var commentFindByEmail = await commentRepository.GetCommentsByEmailAsync("hoanglong@gmail.com");
foreach (var comment in commentFindByEmail)
{
  Console.WriteLine(comment);
}

Console.WriteLine("=======");
await commentRepository.AprroveCommentAsync(1, true);
Console.WriteLine(commentFindById);

//await commentRepository.DeleteCommentAsync(2);

#endregion