using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common.Conventions;

/// <summary>
/// Common EF Core conventions shared across providers.
/// </summary>
internal sealed class ModelConventionPackCommon : IModelConventionPackCommon
{
    /// <summary>
    /// Applies GUIDv7 as the default generator for single Guid primary keys.
    /// Skips key properties with database defaults or explicit generators.
    /// </summary>
    public void UseGuidV7PrimaryKeys(ModelBuilder builder)
    {
        // Convention: if a PK is a single Guid without a DB default, generate GUIDv7 client-side.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()) continue;
            if (entityType.IsKeyless) continue;

            var clrType = entityType.ClrType;
            if (clrType is null) continue;

            var key = entityType.FindPrimaryKey();
            if (key is null || key.Properties.Count != 1) continue;

            var keyProperty = key.Properties[0];
            if (keyProperty.ClrType != typeof(Guid)) continue;

            if (keyProperty.GetDefaultValueSql() is not null || keyProperty.GetDefaultValue() is not null) continue;
            if (keyProperty.GetValueGeneratorFactory() is not null) continue;

            // Skip properties EF cannot set (no setter and no backing field).
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
}
