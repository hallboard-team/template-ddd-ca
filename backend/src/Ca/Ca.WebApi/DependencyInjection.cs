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
        services.AddSuperAdminSeedOptions(config);

        services.AddDataProtection(); // runtime service needed by Identity token providers
        services.AddPolicyService(); // Authorization policies
        services.AddCorsService(); // CORS policies
        services.AddRateLimitingService(); // Global and partitioned throttling
    }

        services.AddInfrastructureServices(config, env);
        services.AddApplicationServices();

        return services;
    }
}