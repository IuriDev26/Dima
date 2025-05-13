using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("{id:int}", HandlerAsync)
            .WithSummary("Get Categories By Id")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandlerAsync(ITransactionHandler handler, long id)
    {
        var request = new GetByIdRequest()
        {
            Id = id,
            UserId = "Iuri"
        };

        var response = await handler.GetByIdAsync(request);
        return response.IsSuccess 
            ? TypedResults.Ok(response) 
            : TypedResults.NotFound(response);
    }
}