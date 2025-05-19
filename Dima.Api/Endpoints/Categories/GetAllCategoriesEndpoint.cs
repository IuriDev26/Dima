using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", HandlerAsync)
            .WithSummary("Gets All Categories")
            .WithOrder(3)
            .Produces<Response<List<Category?>>>();
    
    
    private static async Task<IResult> HandlerAsync(ICategoryHandler handler, 
        [FromQuery] int? pageNumber, 
        [FromQuery] int? pageSize, 
        ClaimsPrincipal user)
    {
        var request = new GetAllCategoriesRequest()
        {
            PageNumber = pageNumber ?? Configuration.DefaultPageNumber,
            PageSize = pageSize ?? Configuration.DefaultPageSize,
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var response = await handler.GetAllAsync(request);
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.BadRequest(response);
    }
}