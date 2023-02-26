﻿using Microsoft.EntityFrameworkCore.Storage;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var context = new BlogDbContext();
var seeder = new DataSeeder(context);

seeder.Initialize();

#region phan4
//var authors = context.Authors.ToList();

//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");

//foreach (var author in authors)
//{
//  Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/DatabaseDependencies/yyyy}",
//    author.Id, author.FullName, author.Email, author.JoinedDate);
//}
#endregion


#region phan 5
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

#region phan6
IBlogRepository blogRepo = new BlogRepository(context);
var posts = await blogRepo.GetPopularArticlesAsync(3);
#endregion

foreach (var post in posts)
{
  Console.WriteLine("Id      : {0}", post.Id);
  Console.WriteLine("Title   : {0}", post.Title);
  Console.WriteLine("View    : {0}", post.ViewCount);
  Console.WriteLine("Date    : {0:MM/dd/yyyy}", post.PostedDate);
  Console.WriteLine("Author  : {0}", post.Author);
  Console.WriteLine("Category: {0}", post.Category);
  Console.WriteLine("".PadRight(80, '-'));
}

