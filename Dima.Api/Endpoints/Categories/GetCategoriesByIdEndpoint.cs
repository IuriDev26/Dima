
using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoriesByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("{id:int}", HandlerAsync)
            .WithSummary("Get Categories By Id")
            .WithOrder(2)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandlerAsync(ICategoryHandler handler, long id, ClaimsPrincipal user)
    {
        var request = new GetCategoryByIdRequest()
        {
            Id = id,
            UserId = user.Identity?.Name ?? string.Empty
        };

        var response = await handler.GetByIdAsync(request);
        return response.IsSuccess 
            ? TypedResults.Ok(response) 
            : TypedResults.NotFound(response);
    }
    
}