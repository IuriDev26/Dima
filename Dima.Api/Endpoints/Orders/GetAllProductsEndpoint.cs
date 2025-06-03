using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Orders;

public class GetAllProductsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", HandleAsync)
            .WithName("Get All Products")
            .Produces<Response<Product?>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IProductHandler handler,
        [FromQuery] int pageSize = Configuration.DefaultPageSize,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber)
    {

        var request = new GetAllProductsRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await handler.GetAllAsync(request);

        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}