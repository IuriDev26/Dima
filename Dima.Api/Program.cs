using System.Security.Claims;
using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Api.Models.Identity;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
}).AddIdentityCore<User>()
.AddRoles<IdentityRole<long>>()
.AddEntityFrameworkStores<AppDbContext>()
.AddApiEndpoints();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapEndpoints();

app.MapGroup("/v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup("/v1/identity")
    .WithTags("Identity")
    .MapGet("/logout", async (SignInManager<User> user) =>
    {
        await user.SignOutAsync();
        return Results.Ok();
    });

app.MapGroup("/v1/identity")
    .WithTags("Identity")
    .MapGet("/roles", (ClaimsPrincipal user) =>
    {
        var identity = (ClaimsIdentity)user.Identity!;
        
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();
            
        var roles = identity.FindAll(identity.RoleClaimType).Select(c => new
        {
            c.Issuer,
            c.OriginalIssuer,
            c.Type,
            c.Value,
            c.ValueType
        });
        
        return TypedResults.Json(roles);
    });


app.Run();


