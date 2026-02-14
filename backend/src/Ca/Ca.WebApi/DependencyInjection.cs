using Ca.Application;
using Ca.Infrastructure;
using Ca.WebApi.Extensions;
using Ca.WebApi.Startup;

namespace Ca.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services, IConfiguration config, IHostEnvironment env
    )
    {
        AddWebApiPlatformServices(services, config);
        AddLayerDependencies(services, config, env);
        return services;
    }

    private static void AddWebApiPlatformServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSuperAdminSeedOptions(config);
        services.AddDataProtection(); // runtime service needed by Identity token providers
        services.AddAuthService(); // Authentication primitives
        services.AddPolicyService(); // Authorization policies
        services.AddCorsService(); // CORS policies
        services.AddRateLimitingService(); // Global and partitioned throttling
    }

    private static void AddLayerDependencies(
        IServiceCollection services, IConfiguration config, IHostEnvironment env
    )
    {
        services.AddInfrastructureServices(config, env);
        services.AddApplicationServices();
    }
}
