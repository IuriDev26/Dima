using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Orders;

public class PayOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("pay", HandleAsync)
            .WithName("Pay Order")
            .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(PayOrderRequest request, ClaimsPrincipal user, 
        IOrderHandler handler)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var response = await handler.PayAsync(request);

        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}