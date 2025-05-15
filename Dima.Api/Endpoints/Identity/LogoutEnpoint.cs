using Dima.Api.Common.Api;
using Dima.Api.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Endpoints.Identity;

public class LogoutEnpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/logout", HandlerAsync)
        .WithSummary("Logout");

    private static async Task<IResult> HandlerAsync(SignInManager<User> user)
    {
        await user.SignOutAsync();
        return Results.Ok("/");
    }
}