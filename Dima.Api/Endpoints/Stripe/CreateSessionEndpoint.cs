using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Stripe;

public class CreateSessionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("create-session", HandleAsync)
            .WithName("Create Stripe Session")
            .Produces<Response<string?>>();

    private static async Task<IResult> HandleAsync(CreateSessionRequest request, IStripeHandler stripeHandler,
        ClaimsPrincipal user)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var response = await stripeHandler.CreateSessionAsync(request);
        
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
    
}