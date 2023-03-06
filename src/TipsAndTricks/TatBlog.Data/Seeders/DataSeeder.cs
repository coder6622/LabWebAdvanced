using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
  public class DataSeeder : IDataSeeder
  {
    private readonly BlogDbContext _dbContext;

    public DataSeeder(BlogDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public void Initialize()
    {
      _dbContext.Database.EnsureCreated();

      //if (_dbContext.Posts.Any())
      //  return;

      var authors = AddAuthors();
      var categories = AddCategories();
      var tags = AddTags();
      var posts = AddPosts(authors, categories, tags);
      var subscribers = AddSubscribers();
    }

    private IList<Author> AddAuthors()
    {

      var authors = new List<Author>()
      {
        new()
        {
          FullName = "Jason Mouth",
          UrlSlug = "jason-mouth",
          Email = "json@gmail.com",
          JoinedDate = new DateTime(2022, 10, 21)
        },
        new()
        {
          FullName = "Jessica Wonder",
          UrlSlug = "jessica-wonder",
          Email = "jessica65@motip.com",
          JoinedDate = new DateTime(2020, 4, 19)
        },
        new()
        {
          FullName = "Hoang Long",
          UrlSlug = "Hoang-long",
          Email = "hoanglong@motip.com",
          JoinedDate = new DateTime(2022, 4, 19)
        }
      };

      foreach (var author in authors)
      {
        if (!_dbContext.Authors.Any(a => a.UrlSlug == author.UrlSlug))
        {
          _dbContext.Authors.Add(author);
        }
      }

      _dbContext.SaveChanges();

      return authors;
    }

    private IList<Category> AddCategories()
    {
      var categories = new List<Category>() {
        new(){Name = ".NET Core", Description = ".NET Core", UrlSlug="net-core"},
        new(){Name = "Architecture", Description = "Architecture", UrlSlug="architecture"},
        new(){Name = "Messaging", Description = "Messaging", UrlSlug="messaging"},
        new(){Name = "OOP", Description = "Object-Oriented Programming", UrlSlug="oop"},
        new(){Name = "Design Patterns", Description = "Design Patterns", UrlSlug="design-patterns"},
        new(){Name = "Web Advanced", Description = "Web advanced", UrlSlug="web-advanced"},
      };

      foreach (var category in categories)
      {
        if (!_dbContext.Categories.Any(c => c.UrlSlug == category.UrlSlug))
        {
          _dbContext.Categories.Add(category);
        }
      }

      _dbContext.SaveChanges();
      return categories;
    }


    private IList<Tag> AddTags()
    {
      var tags = new List<Tag>() {
        new(){Name = "Google", Description = "Google applications", UrlSlug="google-apps"},
        new(){Name = "ASP .NET MVC", Description = "ASP .NET MVC", UrlSlug="asp-net-mvc"},
        new(){Name = "Razor Page", Description = "Razor Page", UrlSlug="razor-page"},
        new(){Name = "Blazor", Description = "Blazor", UrlSlug="blazor"},
        new(){Name = "Deep Learning", Description = "Deep Lerning", UrlSlug="deep-learning"},
        new(){Name = "Neural Network", Description = "Neural Network", UrlSlug="neural-network"},
      };


      foreach (var tag in tags)
      {
        if (!_dbContext.Tags.Any(t => t.UrlSlug == tag.UrlSlug))
        {
          _dbContext.Tags.Add(tag);
        }
      }

      _dbContext.SaveChanges();
      return tags;
    }

    private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags)
    {
      var posts = new List<Post>() {
        new() {
          Title = "ASP .NET Core Diagnostic Scenarios",
          ShortDescription = "David and friends has a great repository",
          Description = "Here's a few great DON'T and DO emaples",
          Meta = "David and friends has a greate repository filled",
          UrlSlug = "aspnet-core-diagnostic-scenarios",
          Published = true,
          PostedDate = new DateTime(2021,9,30,10,20,0),
          ModifiedDate = null,
          ViewCount = 10,
          Author = authors[0],
          Category = categories[0],
          Tags = new List<Tag>(){tags[0]}
        },
        new() {
          Title = "ASP .NET CORE With Razor Page and Blazor",
          ShortDescription = "Long and friends has a great repository",
          Description = "This's the wonderful day",
          Meta = "Long and friends has a greate repository filled",
          UrlSlug = "aspnet-core-razor-blazor",
          Published = true,
          PostedDate = new DateTime(2022,10,30,10,20,0),
          ModifiedDate = null,
          ViewCount = 100,
          Author = authors[1],
          Category = categories[0],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3]}
        },
        new() {
          Title = "ASP .NET CORE Reactjs",
          ShortDescription = "Duat and friends has a great repository",
          Description = "This's good day",
          Meta = "Duat and friends has a greate repository filled",
          UrlSlug = "aspnet-core-reactjs",
          Published = true,
          PostedDate = new DateTime(2022,9,30,10,20,0),
          ModifiedDate = null,
          ViewCount = 200,
          Author = authors[2],
          Category = categories[2],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3], tags[4], tags[5] }
        },
        new() {
          Title = "ASP .NET CORE Reactjs1",
          ShortDescription = "Duat and friends has a great repository",
          Description = "This's good day",
          Meta = "Duat and friends has a greate repository filled",
          UrlSlug = "aspnet-core-reactjs1",
          Published = true,
          PostedDate = new DateTime(2021,5,30,10,20,0),
          ModifiedDate = null,
          ViewCount = 200,
          Author = authors[2],
          Category = categories[2],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3], tags[4], tags[5] }
        },
        new() {
          Title = "ASP .NET CORE Reactjs2",
          ShortDescription = "Duat and friends has a great repository",
          Description = "This's good day",
          Meta = "Duat and friends has a greate repository filled",
          UrlSlug = "aspnet-core-reactjs2",
          Published = true,
          PostedDate = new DateTime(2022,9,30,10,20,0),
          ModifiedDate = null,
          ViewCount = 250,
          Author = authors[2],
          Category = categories[2],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3], tags[4], tags[5] }
        },
        new() {
          Title = "ASP .NET CORE Reactjs3",
          ShortDescription = "Duat and friends has a great repository",
          Description = "This's good day",
          Meta = "Duat and friends has a greate repository filled",
          UrlSlug = "aspnet-core-reactjs3",
          Published = true,
          PostedDate = new DateTime(2021,2,20,10,20,0),
          ModifiedDate = null,
          ViewCount = 200,
          Author = authors[2],
          Category = categories[2],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3], tags[4], tags[5] }
        },
        new() {
          Title = "ASP .NET CORE Reactjs4",
          ShortDescription = "Duat and friends has a great repository",
          Description = "This's good day",
          Meta = "Duat and friends has a greate repository filled",
          UrlSlug = "aspnet-core-reactjs4",
          Published = true,
          PostedDate = new DateTime(2022,6,30,10,20,0),
          ModifiedDate = null,
          ViewCount = 200,
          Author = authors[2],
          Category = categories[2],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3], tags[4], tags[5] }
        },
        new() {
          Title = "ASP .NET CORE Reactjs5",
          ShortDescription = "Duat and friends has a great repository",
          Description = "This's good day",
          Meta = "Duat and friends has a greate repository filled",
          UrlSlug = "aspnet-core-reactjs5",
          Published = true,
          PostedDate = new DateTime(2022,5,12,10,20,0),
          ModifiedDate = null,
          ViewCount = 200,
          Author = authors[2],
          Category = categories[2],
          Tags = new List<Tag>(){tags[1], tags[2], tags[3], tags[4], tags[5] }
        },
      };

      foreach (var post in posts)
      {
        if (!_dbContext.Posts.Any(p => p.UrlSlug == p.UrlSlug))
        {
          _dbContext.Posts.Add(post);
        }
      }
      _dbContext.SaveChanges();
      return posts;
    }

    private IList<Subscriber> AddSubscribers()
    {
      var subscribers = new List<Subscriber>() {
        new()
        {
          Email = "hoanglong@gmail.com",
          SubscribedDate = new DateTime(2023,3,4,3,20,0),
        },
        new()
        {
          Email = "nhatduat@gmail.com",
          SubscribedDate = new DateTime(2023,3,2,7,0,0),
        },
        new()
        {
          Email = "xuanhung@gmail.com",
          SubscribedDate = new DateTime(2023,3,1,8,0,0),
        },
        new()
        {
          Email = "minhtien@gmail.com",
          SubscribedDate = new DateTime(2023,2,28,9,15,0),
        },
        new()
        {
          Email = "trunghieu@gmail.com",
          SubscribedDate = new DateTime(2023,2,28,20,05,0),
        },
      };
      foreach (var subscriber in subscribers)
      {
        if (!_dbContext.Subscribers.Any(s => s.Email == subscriber.Email))
        {
          _dbContext.Subscribers.Add(subscriber);
        }
      }
      _dbContext.SaveChanges();
      return subscribers;
    }
  }
}
