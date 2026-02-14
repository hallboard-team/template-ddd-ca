namespace Ca.WebApi.Extensions;

internal static class AuthExtensions
{
    internal static IServiceCollection AddAuthService(this IServiceCollection services)
    {
        services.AddAuthentication(); // TODO: Impolement further
        return services;
    }
}
