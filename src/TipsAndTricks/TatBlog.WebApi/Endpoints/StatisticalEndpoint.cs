using Carter;
using System.Runtime.CompilerServices;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
  public class StatisticalEndpoint : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      var routeGroupBuilder = app.MapGroup("/api/statistical");

      routeGroupBuilder.MapGet("/", GetStatistical)
        .WithName("GetStatistical")
        .Produces<ApiResponse<StatisticalItem>>();
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
