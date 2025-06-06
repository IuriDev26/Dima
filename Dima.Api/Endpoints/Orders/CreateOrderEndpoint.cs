using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders;

public class CreateOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("create", HandleAsync)
            .WithName("Create Order")
            .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(CreateOrderRequest request, ClaimsPrincipal user, IOrderHandler handler)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        request.VoucherId = request.VoucherId;
        request.ProductId = request.ProductId;

        var response = await handler.CreateAsync(request);

        return response.IsSuccess
            ? TypedResults.Created<Response<Order?>>($"$/orders/{response.Data!.Number}", response)
            : TypedResults.BadRequest(response);
    }
}