using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetIncomesAndExpensesReportEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("incomes-and-expenses", HandlerAsync)
            .WithName("Get Incomes and Expenses Report")
            .Produces<Response<List<IncomesAndExpenses>?>>();
    
    private static async Task<IResult> HandlerAsync(ClaimsPrincipal user,
        IReportHandler handler)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return TypedResults.Unauthorized();
        
        var request = new GetIncomesAndExpensesRequest()
        {
            UserId = user.Identity.Name!
        };
        var response = await handler.GetIncomesAndExpensesReportAsync(request);
        
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    } 
}