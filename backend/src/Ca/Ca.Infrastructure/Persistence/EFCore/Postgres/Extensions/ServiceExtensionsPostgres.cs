using Ca.Infrastructure.Persistence.EFCore.Common;
using Ca.Infrastructure.Persistence.EFCore.Common.Conventions;
using Ca.Infrastructure.Persistence.EFCore.Postgres.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Extensions;

/// <summary>
///     Set db config at Runtime [DI]
///     Also have a Design-Time at factory for non-running app migrations. See AppDbContextPostgresFactory.cs
/// </summary>
internal static class ServiceExtensionsPostgres
{
    internal static IServiceCollection AddConfigsServicePostgres(
        this IServiceCollection services, IConfiguration config
    )
    {
        services.AddOptions<MyPostgresSettings>().Bind(config.GetSection(nameof(MyPostgresSettings))).Validate(
            settings => !string.IsNullOrWhiteSpace(settings.ConnectionString),
            "Postgres ConnectionString is required."
        ).ValidateOnStart(); // Fail fast at startup

        return services;
    }

    /// <summary>
    ///     Configures and registers the <see cref="AppDbContextPostgres" /> with PostgreSQL as the underlying database
    ///     provider.
    ///     This method:
    ///     <list type="bullet">
    ///         <item>
    ///             Binds the connection string from <see cref="MyPostgresSettings" /> and throws a
    ///             <see cref="InvalidOperationException" />
    ///             if not found.
    ///         </item>
    ///         <item>
    ///             Enables retry-on-failure for transient errors (5 retries, 10 seconds max delay).
    ///         </item>
    ///         <item>
    ///             Uses a dedicated migrations assembly (<c>Ca.Infrastructure.Persistence.EFCore.Postgres</c>) to keep
    ///             provider-specific migrations isolated.
    ///         </item>
    ///         <item>
    ///             Sets the default query tracking behavior to <see cref="QueryTrackingBehavior.NoTracking" /> for read
    ///             performance.
    ///         </item>
    ///         <item>
    ///             Enables detailed errors and sensitive data logging in <c>Development</c> only; explicitly disables them in
    ///             Production.
    ///         </item>
    ///         <item>
    ///             Configures EF Core to throw an exception on
    ///             <see cref="RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning" /> to prevent subtle
    ///             LINQ-to-Entities bugs.
    ///         </item>
    ///     </list>
    /// </summary>
    /// <param name="services">The application service collection.</param>
    /// <param name="env"></param>
    /// <returns>The modified service collection.</returns>
    /// <exception cref="NullReferenceException">Thrown when the PostgreSQL connection string is not configured.</exception>
    public static IServiceCollection AddServicePostgres(
        this IServiceCollection services, IHostEnvironment env
    )
    {
        // Register CustomModelBuilder
        services.AddScoped<IModelConventionPackCommon, ModelConventionPackCommon>();
        services.AddScoped<IModelConventionPackPostgres, ModelConventionPackPostgres>();

        // AddDbContextPool has more performance than AddDbContext
        services.AddDbContextPool<AppDbContextPostgres>((provider, options) =>
            {
                string connectionStringRaw = provider.GetRequiredService<IOptions<MyPostgresSettings>>().Value.
                                                 ConnectionString
                                             ?? throw new InvalidOperationException(
                                                 "Postgres ConnectionString is required."
                                             );

                var connectionBuilder = new NpgsqlConnectionStringBuilder(connectionStringRaw);

                if (env.IsProduction())
                {
                    if (connectionBuilder.SslMode < SslMode.Require)
                        connectionBuilder.SslMode = SslMode.Require;
                    // If you manage certs, prefer:
                    // b.SslMode = Npgsql.SslMode.VerifyFull;
                    // b.RootCertificate = cfg["MyPostgresSettings:RootCertificatePath"]; // optional
                }

                string connectionString = connectionBuilder.ConnectionString;

                options.UseNpgsql(
                    connectionString, npg =>
                    {
                        npg.EnableRetryOnFailure(
                            maxRetryCount: 5, TimeSpan.FromSeconds(seconds: 10), errorCodesToAdd: null
                        ); // Retry policy
                        npg.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                    }
                );

                // Default: optimize for reads
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                if (env.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(); // Only in Dev
                    options.EnableDetailedErrors(); // Only in Dev
                }

                // Always-on safety: detect unintended Equals() usage in queries
                options.ConfigureWarnings(warnings => warnings.Throw(
                        RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning
                    )
                );
            }
        );

        return services;
    }
}
