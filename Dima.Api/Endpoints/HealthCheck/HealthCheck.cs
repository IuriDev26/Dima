using Dima.Api.Common.Api;

namespace Dima.Api.Endpoints.HealthCheck;

public class HealthCheck : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", () =>  "Hello World" );
}