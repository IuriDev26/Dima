using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders;

public class RefundOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("refund", HandleAsync)
            .WithName("Refund Order")
            .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(RefundOrderRequest request, ClaimsPrincipal user, 
        IOrderHandler handler)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var response = await handler.RefundAsync(request);

        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}