using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Orders;

public class GetVoucherByNumberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("{number}", HandleAsync);

    private static async Task<IResult> HandleAsync([FromRoute] string? number,
        ClaimsPrincipal user, IVoucherHandler handler)
    {

        var request = new GetVoucherByNumberRequest
        {
            Number = number ?? string.Empty
        };

        var response = await handler.GetByNumberAsync(request);
            
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}