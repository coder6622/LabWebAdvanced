using System.Runtime.CompilerServices;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
  public static class StatisticalEndpoint
  {

    public static WebApplication MapStatisticalEndpoint(this WebApplication app)
    {
      var routeGroupBuilder = app.MapGroup("/api/statistical");

      routeGroupBuilder.MapGet("/", GetStatistical)
        .WithName("GetStatistical")
        .Produces<ApiResponse<StatisticalItem>>();

      return app;
    }

    private static async Task<IResult> GetStatistical(
      IStatisticalRepository statisticalRepository)
    {

      var statistical = await statisticalRepository.GetStatisticals();

      return Results.Ok(ApiResponse.Success(
        statistical));
    }
  }
}
