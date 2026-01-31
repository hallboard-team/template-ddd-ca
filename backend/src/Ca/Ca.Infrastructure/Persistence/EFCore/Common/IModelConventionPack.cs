using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common;

public interface IModelConventionPack
{
    void UseGuidV7PrimaryKeys(ModelBuilder builder);
    void UseOptimisticConcurrencyWithXmin(ModelBuilder builder);
}
