using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Ca.Infrastructure.Persistence.EFCore.Common.Conventions;

/// <summary>
/// Generates GUIDv7 values for client-side primary keys.
/// </summary>
internal sealed class GuidV7ValueGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry) => Guid.CreateVersion7();

    public override bool GeneratesTemporaryValues => false;
}
