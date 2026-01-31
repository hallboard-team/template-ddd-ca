using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Conventions;

public interface IModelConventionPackPostgres
{
    /// <summary>
    /// Applies Postgres-specific optimistic concurrency using the xmin system column.
    /// </summary>
    void UseOptimisticConcurrencyWithXmin(ModelBuilder builder);
}
