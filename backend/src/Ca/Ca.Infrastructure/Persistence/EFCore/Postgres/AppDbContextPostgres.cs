using Ca.Infrastructure.Modules.Auth.Postgres.Models;
using Ca.Infrastructure.Persistence.EFCore.Common;
using Ca.Infrastructure.Persistence.EFCore.Postgres.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres;

// General settings for Postgres using EFCore
public class AppDbContextPostgres(IModelConventionPackCommon commonConventionPack,
    IModelConventionPackPostgres postgresConventionPack,
    DbContextOptions<AppDbContextPostgres> options) : DbContext(options)
{
    // Postgres-compatible aggregates
    public DbSet<AppUserPostgres> Users => Set<AppUserPostgres>();

    /// <summary>
    ///     Separate configuration classes
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Customized configuration classes
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContextPostgres).Assembly);

        // Applied our custom conventions
        ApplyConventions(builder);

        // Allow base DbContext conventions/config to run. 
        base.OnModelCreating(builder);
    }

    private void ApplyConventions(ModelBuilder builder)
    {
        commonConventionPack.UseGuidV7PrimaryKeys(builder); // Default GUIDv7 for single Guid primary keys
        postgresConventionPack.UseOptimisticConcurrencyWithXmin(builder); // Global xmin, with opt-outs
    }
}
