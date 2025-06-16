using System.Text.Json.Serialization;
using Dima.Api;
using Dima.Api.Common.Extensions;
using Dima.Api.Endpoints;
using Microsoft.AspNetCore.Http.Json;

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
app.UseSecurity();
app.Run();


