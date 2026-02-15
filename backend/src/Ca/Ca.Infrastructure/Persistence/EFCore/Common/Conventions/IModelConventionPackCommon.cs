using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common.Conventions;

/// <summary>
/// Provider-agnostic EF Core conventions.
/// </summary>
public interface IModelConventionPackCommon
{
    internal void UseGuidV7PrimaryKeys(ModelBuilder builder);
}
