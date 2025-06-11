using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders;

public class GetProductByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("{id:long}", HandleAsync)
            .WithName("Get Product By Id")
            .Produces<Response<Product?>>();

    private static async Task<IResult> HandleAsync(long id, ClaimsPrincipal user, IProductHandler handler)
    {
        var request = new GetProductByIdRequest(id);
        var response = await handler.GetByIdAsync(request);

        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}