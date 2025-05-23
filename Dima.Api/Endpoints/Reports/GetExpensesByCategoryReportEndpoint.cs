using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetExpensesByCategoryReportEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("expenses-by-category", HandlerAsync)
            .WithName("Get Expenses By Category Report")
            .Produces<Response<List<ExpensesByCategory>?>>();

    private static async Task<IResult> HandlerAsync(ClaimsPrincipal user,
        IReportHandler handler)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return TypedResults.Unauthorized();
        
        var request = new GetExpensesByCategoryRequest()
        {
            UserId = user.Identity.Name!
        };
        var response = await handler.GetExpensesByCategoryReportAsync(request);
        
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}