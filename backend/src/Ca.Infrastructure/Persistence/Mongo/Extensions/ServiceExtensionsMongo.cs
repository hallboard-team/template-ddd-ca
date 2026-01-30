using Ca.Infrastructure.Persistence.Mongo.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ca.Infrastructure.Persistence.Mongo.Extensions;

internal static class ServiceExtensionsMongo
{
    internal static IServiceCollection AddConfigsServiceMongo(
        this IServiceCollection services, IConfiguration config
    )
    {
        services.AddOptions<MyMongoDbSettings>().Bind(config.GetSection(nameof(MyMongoDbSettings))).Validate(
            settings => !string.IsNullOrWhiteSpace(settings.ConnectionString) &&
                        !string.IsNullOrWhiteSpace(settings.DatabaseName),
            "MongoDB ConnectionString and DatabaseName are required."
        ).ValidateOnStart(); // Fail fast at startup

        return services;
    }

    internal static IServiceCollection AddServiceMongo(this IServiceCollection services)
    {
        // get values
        services.AddSingleton<IMyMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MyMongoDbSettings>>().Value
        );

        // get connectionString to the db
        services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                MyMongoDbSettings myMongoDbSettings = serviceProvider.GetRequiredService<IOptions<MyMongoDbSettings>>().
                    Value;

                return new MongoClient(myMongoDbSettings.ConnectionString);
            }
        );

        return services;
    }
}