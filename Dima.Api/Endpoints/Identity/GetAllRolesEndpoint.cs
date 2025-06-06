using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Models.Identity;

namespace Dima.Api.Endpoints.Identity;

public class GetAllRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/roles", Handler)
        .WithSummary("Returns a list of all roles.");

    private static IResult Handler(ClaimsPrincipal user)
    {
        if ( user.Identity is null || !user.Identity.IsAuthenticated )
            return Results.Unauthorized();
        
        var identity = (ClaimsIdentity)user.Identity;
        
        var roles = identity.FindAll(identity.RoleClaimType).Select(c => new RoleClaim
        {
            Issuer = c.Issuer,
            OriginalIssuer = c.OriginalIssuer,
            Type = c.Type,
            Value = c.Value,
            ValueType = c.ValueType
        });
        
        return TypedResults.Json(roles);
    }
}