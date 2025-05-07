using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandlerAsync)
            .WithSummary("Creates a Category")
            .WithOrder(1)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandlerAsync(CreateCategoryRequest request, ICategoryHandler handler)
    {
            var response = await handler.CreateAsync(request);
            return response.IsSuccess 
                ? TypedResults.Created($"/v1/categories/{response.Data?.Id}", response) 
                : TypedResults.BadRequest(response);
    }
}