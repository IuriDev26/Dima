using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandlerAsync)
            .WithSummary("Gets All Categories")
            .WithOrder(3)
            .Produces<Response<List<Transaction?>>>();
    
    
    private static async Task<IResult> HandlerAsync(ITransactionHandler handler, 
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        ClaimsPrincipal user) 
    {
        var request = new GetByPeriodRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = user.Identity?.Name ?? string.Empty,
            FinalInterval = endDate,
            InitialInterval = startDate,
        };
        
        var response = await handler.GetByPeriodAsync(request);
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response.Data);
    }
}