namespace Dima.Api.Common.Extensions;

public static class AppExtension
{
    public static void AddSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseCors(Configuration.CorsPolicyName);
        app.UseAuthentication();
        app.UseAuthorization();

    }
}