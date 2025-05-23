using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Api.Models.Identity;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetFinancialSummaryReportEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("financial-summary", HandlerAsync)
            .WithName("Get Financial Summary Report")
            .Produces<Response<List<FinancialSummary>?>>();

    private static async Task<IResult> HandlerAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return TypedResults.Unauthorized();
        
        var request = new GetFinancialSummaryRequest()
        {
            UserId = user.Identity.Name!
        };
        var response = await handler.GetFinancialSummaryReportAsync(request);
        
        return response.IsSuccess 
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
    
}