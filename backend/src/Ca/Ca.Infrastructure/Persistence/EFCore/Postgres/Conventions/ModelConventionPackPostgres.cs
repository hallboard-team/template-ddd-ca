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

    public void ApplyTenantGlobalFilters(ModelBuilder builder)
    {
        // foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
        // {
        //     if (entityType.IsOwned()) continue;
        //     if (entityType.IsKeyless) continue;

        //     Type clrType = entityType.ClrType;
        //     if (!typeof(ITenantScoped).IsAssignableFrom(clrType)) continue;

        //     ApplyTenantGlobalFilterForEntity(builder, clrType);
        // }
    }

//     private void ApplyTenantGlobalFilterForEntity(ModelBuilder builder, Type clrType)
//     {
//         var method = typeof(AppDbContextPostgres).GetMethod(
//             nameof(ApplyTenantGlobalFilterForEntityGeneric),
//             System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
//         ) ?? throw new InvalidOperationException("Tenant filter method not found.");

//         method.MakeGenericMethod(clrType).Invoke(this, [builder]);
//     }

//     // Fail-closed by default: if tenant context is missing, return zero tenant-scoped rows.
//     private void ApplyTenantGlobalFilterForEntityGeneric<TEntity>(ModelBuilder builder)
//         where TEntity : class, ITenantScoped
//     {
//         builder.Entity<TEntity>()
//             .HasQueryFilter(entity =>
//             CurrentTenantId.HasValue && entity.TenantId == CurrentTenantId.Value
//         );

//         builder.Entity<Project>()
//   .HasQueryFilter(p => p.TenantId == _tenantContext.TenantId);

//     }
}
