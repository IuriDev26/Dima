using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("{id:int}", HandlerAsync)
            .WithSummary("Update Category")
            .WithOrder(4)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandlerAsync(UpdateCategoryRequest request, ICategoryHandler handler, long id)
    {
        request.Id = id;
        request.UserId = "Iuri";
        
        var response = await handler.UpdateAsync(request);
        return response.IsSuccess
            ? TypedResults.Ok(response.Data)
            : TypedResults.BadRequest(response.Data);
    }
}