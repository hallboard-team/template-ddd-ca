using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ca.Infrastructure.Persistence.EFCore.Common;

public interface IModelConventionPack
{
    void UseOptimisticConcurrencyWithXmin<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class;
}