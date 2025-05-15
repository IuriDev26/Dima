using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models.Identity;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Common.Extensions;

public static class BuilderExtension
{

    public static void AddConfiguraions(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                                         ?? string.Empty;
        Configuration.FrontEndUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
        Configuration.FrontEndUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
    }
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
        builder.Services.AddAuthorization();
    }

    public static void AddDbContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.ConnectionString);
            })
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(Configuration.CorsPolicyName,
            policy => policy
                .WithOrigins([
                    Configuration.FrontEndUrl,
                    Configuration.BackEndUrl 
                ])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));
    }
}