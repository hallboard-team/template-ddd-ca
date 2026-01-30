using Ca.Infrastructure.Persistence.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Extensions;

internal sealed class ModelConventionPackPostgres : IModelConventionPack
{
    /// <summary>
    /// Maps Postgres system column xmin as a concurrency token.
    /// </summary>
    public void UseOptimisticConcurrencyWithXmin<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class =>
        builder.Property<uint>("xmin") // shadow map to system column
            .IsRowVersion(); // concurrency token + DB-generated on add/update
}
