using Ca.Domain.Modules.Common.Base;
using Ca.Infrastructure.Persistence.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Extensions;

internal sealed class ModelConventionPackPostgres : IModelConventionPack
{
    public void UseGuidV7PrimaryKeys(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()) continue;
            if (entityType.IsKeyless) continue;

            var clrType = entityType.ClrType;
            if (clrType is null) continue;
            if (clrType.IsAbstract) continue;

            var key = entityType.FindPrimaryKey();
            if (key is null || key.Properties.Count != 1) continue;

            var keyProperty = key.Properties[0];
            if (keyProperty.ClrType != typeof(Guid)) continue;

            if (keyProperty.GetDefaultValueSql() is not null || keyProperty.GetDefaultValue() is not null) continue;
            if (keyProperty.GetValueGeneratorFactory() is not null) continue;

            var propertyInfo = keyProperty.PropertyInfo;
            var fieldInfo = keyProperty.FieldInfo;
            if (propertyInfo is null && fieldInfo is null) continue;
            if (propertyInfo is not null && propertyInfo.SetMethod is null && fieldInfo is null) continue;

            builder.Entity(clrType)
                   .Property<Guid>(keyProperty.Name)
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator<GuidV7ValueGenerator>();
        }
    }

    /// <summary>
    /// Maps Postgres system column xmin as a concurrency token.
    /// </summary>
    public void UseOptimisticConcurrencyWithXmin(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()) continue;
            if (entityType.IsKeyless) continue;

            var clrType = entityType.ClrType;
            if (clrType is null) continue;
            if (!ShouldUseXmin(clrType)) continue;

            builder.Entity(clrType).Property<uint>("xmin").IsRowVersion();
        }
    }

    private static bool ShouldUseXmin(Type clrType) =>
        !typeof(IAppendOnly).IsAssignableFrom(clrType) && !XminExcludedTypes.Contains(clrType);
}
