using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetIncomesByCategoryReportEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("incomes-by-category", HandlerAsync)
            .WithName("Get Incomes By Category Report")
            .Produces<Response<List<IncomesByCategory>?>>();

    private static async Task<IResult> HandlerAsync(ClaimsPrincipal user,
        IReportHandler handler)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return TypedResults.Unauthorized();

        var request = new GetIncomesByCategoryRequest()
        {
            UserId = user.Identity.Name!
        };
        
        var response = await handler.GetIncomesByCategoryReportAsync(request);
        
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}