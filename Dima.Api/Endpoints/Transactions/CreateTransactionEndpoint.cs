using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandlerAsync)
            .WithSummary("Creates a Transacation")
            .WithOrder(1)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandlerAsync(CreateTransactionRequest request, ITransactionHandler handler)
    {
        var response = await handler.CreateAsync(request);
        return response.IsSuccess 
            ? TypedResults.Created($"/v1/categories/{response.Data?.Id}", response) 
            : TypedResults.BadRequest(response);
    }
}