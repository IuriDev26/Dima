using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Orders;

public class GetOrderByNumberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("{number}", HandleAsync)
            .WithName("Get Order By Number").
            Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync([FromRoute]string number, ClaimsPrincipal user, IOrderHandler handler)
    {
        var request = new GetOrderByNumberRequest(number)
        {
            UserId = user.Identity?.Name ?? string.Empty
        };

        var response = await handler.GetByNumberAsync(request);
        
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}