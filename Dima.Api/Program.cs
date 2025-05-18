using Dima.Api;
using Dima.Api.Common.Extensions;
using Dima.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfigurations();
builder.AddSecurity();
builder.AddDbContexts();
builder.AddSwagger();
builder.AddServices();
builder.AddCrossOrigin();

var app = builder.Build();
app.AddSwagger();
app.MapEndpoints();
app.UseCors(Configuration.CorsPolicyName);
app.UseSecurity();
app.Run();


