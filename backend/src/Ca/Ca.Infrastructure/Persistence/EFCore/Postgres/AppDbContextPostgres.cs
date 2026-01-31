using Ca.Infrastructure.Modules.Auth.Postgres.Models;
using Ca.Infrastructure.Persistence.EFCore.Common;
using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres;

// General settings for Postgres using EFCore
public class AppDbContextPostgres(IModelConventionPack conventionPack,
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
        conventionPack.UseGuidV7PrimaryKeys(builder); // Default GUIDv7 for single Guid primary keys
        conventionPack.UseOptimisticConcurrencyWithXmin(builder.Entity<AppUserPostgres>());
        
        // Apply some possible inherited conventions, otherwise EF skips all the extra mappings 
        base.OnModelCreating(builder);
    }
}
