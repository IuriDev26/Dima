using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Stripe;

public class GetTransactionsByOrderNumberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("get-transactions", HandleAsync);

    private static async Task<IResult> HandleAsync([FromQuery] string orderNumber, IStripeHandler stripeHandler,
        ClaimsPrincipal user)
    {
        var request = new GetTransactionsByOrderNumberRequest(orderNumber)
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var response = await stripeHandler.GetTransactionsByOrderNumberAsync(request);
        
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }

}