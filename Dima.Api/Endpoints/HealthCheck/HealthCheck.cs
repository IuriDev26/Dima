using Dima.Api.Common.Api;

namespace Dima.Api.Endpoints.HealthCheck;

public class HealthCheck : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", () =>  $"Hello world - Front end: {Configuration.FrontEndUrl} -  BAckend: {Configuration.BackEndUrl} - Connect: {Configuration.ConnectionString}" );
}