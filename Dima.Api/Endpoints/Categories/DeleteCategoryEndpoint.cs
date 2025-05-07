using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("{id:int}", HandlerAsync)
            .WithSummary("Deletes a Category")
            .WithOrder(5)
            .Produces<Response<Category?>>();
    
    private static async Task<IResult> HandlerAsync(ICategoryHandler handler, long id)
    {
        var request = new DeleteCategoryRequest()
        {
            Id = id,
            UserId = "Iuri"
        };
        
        var response = await handler.DeleteAsync(request);
        return response.IsSuccess
            ? TypedResults.Ok(response.Data)
            : TypedResults.BadRequest(response.Data);
    }
}