using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);

{
  builder
    .ConfigureCors()
    .ConfigureNLog()
    .ConfigureServices()
    .ConfigureSwaggerOpenApi()
    .ConfigureMapster()
    .ConfigureFluentValidation();
}


var app = builder.Build();

{
  app.SetupRequestPipeline();

  app.MapPostEndpoints();

  app.MapCategoryEndpoints();

  app.MapAuthorEndpoints();

  app.Run();
}



