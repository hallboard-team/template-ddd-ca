using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common;

public interface IModelConventionPack
{
    void UseGuidV7PrimaryKeys(ModelBuilder builder);
    void UseOptimisticConcurrencyWithXmin<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class;
}
