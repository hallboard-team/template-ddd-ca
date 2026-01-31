using Ca.Infrastructure.Persistence.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Extensions;

internal sealed class ModelConventionPackPostgres : IModelConventionPack
{
    public void UseGuidV7PrimaryKeys(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()) continue;

            var key = entityType.FindPrimaryKey();
            if (key is null || key.Properties.Count != 1) continue;

            var keyProperty = key.Properties[0];
            if (keyProperty.ClrType != typeof(Guid)) continue;

            if (keyProperty.GetDefaultValueSql() is not null || keyProperty.GetDefaultValue() is not null) continue;

            var clrType = entityType.ClrType;
            if (clrType is null) continue;

            builder.Entity(clrType)
                   .Property<Guid>(keyProperty.Name)
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator<GuidV7ValueGenerator>();
        }
    }

    /// <summary>
    /// Maps Postgres system column xmin as a concurrency token.
    /// </summary>
    public void UseOptimisticConcurrencyWithXmin<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class =>
        builder.Property<uint>("xmin") // shadow map to system column
            .IsRowVersion(); // concurrency token + DB-generated on add/update
}
