using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models.Identity;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Dima.Api.Common.Extensions;

public static class BuilderExtension
{

    public static void AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                                         ?? string.Empty;
        Configuration.FrontEndUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
        Configuration.BackEndUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.StripeApiKey = builder.Configuration.GetValue<string>("StripeApiKey") ?? string.Empty;

        StripeConfiguration.ApiKey = Configuration.StripeApiKey;
    }
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
        
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
        builder.Services.AddTransient<IReportHandler, ReportHandler>();
        builder.Services.AddTransient<IOrderHandler, OrderHandler>();
        builder.Services.AddTransient<IProductHandler, ProductHandler>();
        builder.Services.AddTransient<IVoucherHandler, VoucherHandler>();
        builder.Services.AddTransient<IStripeHandler, StripeHandler>();
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