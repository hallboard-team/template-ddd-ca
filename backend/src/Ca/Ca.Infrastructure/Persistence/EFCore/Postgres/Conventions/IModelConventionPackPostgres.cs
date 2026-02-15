using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Conventions;

public interface IModelConventionPackPostgres
{
    /// <summary>
    /// Applies Postgres-specific optimistic concurrency using the xmin system column.
    /// </summary>
    internal void UseOptimisticConcurrencyWithXmin(ModelBuilder builder);
    internal void ApplyTenantGlobalFilters(ModelBuilder builder);
}
