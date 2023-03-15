using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapster;
using TatBlog.WebApp.Validations;

var builder = WebApplication.CreateBuilder(args);
{
  builder
    .ConfigureMvc()
    .ConfigureNLog()
    .ConfigureServices()
    .ConfigureMapster()
    .ConfigureFluentValidation();
}
var app = builder.Build();
{
  app.UseRequestPipeline();
  app.UseBlogRoutes();
  app.UseDataSeeder();
}

app.Run();
