namespace Ca.WebApi.Extensions;

internal static class AppExtensions
{
    // WebApi specific extensions like Swagger, HealthChecks, etc
    internal static void UseAppServices(this WebApplication app)
    {
        ImplementSwagger(app);
    }

    private static void ImplementSwagger(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Ca.WebApi"); }
            );
        }
    }
}