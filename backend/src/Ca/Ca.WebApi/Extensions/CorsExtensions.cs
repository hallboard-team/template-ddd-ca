namespace Ca.WebApi.Extensions;

internal static class CorsExtensions
{
    internal static IServiceCollection AddCorsService(this IServiceCollection services)
    {
        services.AddCors(); // TODO: Impolement further
        return services;
    }
}
