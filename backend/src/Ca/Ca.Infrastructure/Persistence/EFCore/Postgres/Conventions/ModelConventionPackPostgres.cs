using Ca.Domain.Modules.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Conventions;

/// <summary>
/// Postgres-specific EF Core conventions.
/// </summary>
internal sealed class ModelConventionPackPostgres : IModelConventionPackPostgres
{
    // Infrastructure escape hatch for entities that should not use xmin.
    private static readonly HashSet<Type> XminExcludedTypes = new();

    /// <summary>
    /// Maps Postgres system column xmin as a concurrency token.
    /// </summary>
    public void UseOptimisticConcurrencyWithXmin(ModelBuilder builder)
    {
        // Convention: apply xmin to all eligible entities unless explicitly excluded.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()) continue;
            if (entityType.IsKeyless) continue;

            var clrType = entityType.ClrType;
            if (clrType is null) continue;
            if (clrType.IsAbstract) continue;
            if (!ShouldUseXmin(clrType)) continue;

            builder.Entity(clrType).Property<uint>("xmin").IsRowVersion();
        }
    }

    private static bool ShouldUseXmin(Type clrType) =>
        // Domain opt-out via IAppendOnly, plus infra escape hatch via XminExcludedTypes.
        !typeof(IAppendOnly).IsAssignableFrom(clrType) && !XminExcludedTypes.Contains(clrType);
}
