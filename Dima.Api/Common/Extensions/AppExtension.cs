namespace Dima.Api.Common.Extensions;

public static class AppExtension
{
    public static void AddSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}