using Ca.Infrastructure.Persistence.EFCore.Common.Conventions;
using Ca.Infrastructure.Persistence.EFCore.Postgres.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Npgsql;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres;

/// <summary>
///     Design-Time (instead of Run-Time)
///     Enable migrations without running WebApi to make it api independent.
///     Needs these packages:
///     <PackageReference Include="Microsoft.Extensions.Configuration" />
///     <PackageReference Include="Microsoft.Extensions.Configuration.Json" />
///     <PackageReference Include="Microsoft.Extensions.FileProviders.Physical" />
///     <PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables" />
/// </summary>
public sealed class AppDbContextPostgresFactory : IDesignTimeDbContextFactory<AppDbContextPostgres>
{
    public AppDbContextPostgres CreateDbContext(string[] args)
    {
        // 0) CI/local override
        string? fromEnv = Environment.GetEnvironmentVariable("EFCORE_DESIGNTIME_CONN");
        if (!string.IsNullOrWhiteSpace(fromEnv)) return Build(fromEnv);

        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        string cwd = Directory.GetCurrentDirectory();

        // 1) Optional infra-local design-time file
        var cb = new ConfigurationBuilder();
        string designTime = Path.Combine(cwd, "appsettings.DesignTime.json");
        if (File.Exists(designTime)) cb.AddJsonFile(designTime, optional: false, reloadOnChange: false);

        // 2) Try to load WebApi appsettings.*
        string[] candidates = new[]
        {
            Path.GetFullPath(
                Path.Combine(cwd, "../Ca.WebApi")
            ), // backend/src/Ca.Infrastructure -> backend/src/Ca.WebApi
            Path.GetFullPath(Path.Combine(cwd, "../../src/Ca.WebApi")),
            Path.GetFullPath(Path.Combine(cwd, "../../Ca.WebApi"))
        };
        string? webApiPath = candidates.FirstOrDefault(Directory.Exists);
        if (webApiPath is not null)
        {
            var fp = new PhysicalFileProvider(webApiPath);
            cb.AddJsonFile(fp, "appsettings.json", optional: true, reloadOnChange: false).AddJsonFile(
                fp, $"appsettings.{env}.json", optional: true, reloadOnChange: false
            );
        }

        cb.AddEnvironmentVariables();
        IConfigurationRoot cfg = cb.Build();

        string connectionStringRaw = cfg[$"{nameof(MyPostgresSettings)}:{nameof(MyPostgresSettings.ConnectionString)}"]
                                     ?? throw new InvalidOperationException(
                                         "Postgres connection not found. Expected 'MyPostgresSettings:ConnectionString'."
                                     );

        if (string.Equals(env, "Production", StringComparison.OrdinalIgnoreCase))
        {
            var connectionBuilder = new NpgsqlConnectionStringBuilder(connectionStringRaw);
            if (connectionBuilder.SslMode < SslMode.Require)
                connectionBuilder.SslMode = SslMode.Require;
            // If you manage certs, prefer:
            // b.SslMode = Npgsql.SslMode.VerifyFull;
            // b.RootCertificate = cfg["MyPostgresSettings:RootCertificatePath"]; // optional

            return Build(connectionBuilder.ConnectionString);
        }

        return Build(connectionStringRaw);

        static AppDbContextPostgres Build(string conn)
        {
            DbContextOptions<AppDbContextPostgres> opts = new DbContextOptionsBuilder<AppDbContextPostgres>().
                UseNpgsql(
                    conn, npg =>
                    {
                        npg.EnableRetryOnFailure(
                            maxRetryCount: 5, TimeSpan.FromSeconds(seconds: 10), errorCodesToAdd: null
                        );
                        npg.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                        npg.CommandTimeout(commandTimeout: 30);
                    }
                ).Options;

            return new AppDbContextPostgres(new ModelConventionPackCommon(), new ModelConventionPackPostgres(), opts);
        }
    }
}
