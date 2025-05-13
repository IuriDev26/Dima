using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;


public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("{id:int}", HandlerAsync)
            .WithSummary("Deletes a Transaction")
            .WithOrder(5)
            .Produces<Response<Transaction?>>();
    
    private static async Task<IResult> HandlerAsync(ITransactionHandler handler, long id)
    {
        var request = new DeleteTransactionRequest()
        {
            Id = id,
            UserId = "Iuri"
        };
        
        var response = await handler.DeleteAsync(request);
        return response.IsSuccess
            ? TypedResults.Ok(response.Data)
            : TypedResults.BadRequest(response.Data);
    }
}