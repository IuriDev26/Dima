using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("{pageNumber:int}/{pageSize:int}", HandlerAsync)
            .WithSummary("Gets All Categories")
            .WithOrder(3)
            .Produces<Response<List<Category?>>>();
    
    
    private static async Task<IResult> HandlerAsync(ICategoryHandler handler, 
        int pageNumber, int pageSize)
    {
        var request = new GetAllCategoriesRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = "Iuri"
        };
        
        var response = await handler.GetAllAsync(request);
        return response.IsSuccess
            ? TypedResults.Ok(response.Data)
            : TypedResults.BadRequest(response.Data);
    }
}