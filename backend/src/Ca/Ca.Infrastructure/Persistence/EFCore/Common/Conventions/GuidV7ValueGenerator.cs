using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Ca.Infrastructure.Persistence.EFCore.Common.Conventions;

/// <summary>
/// Generates GUIDv7 values for client-side primary keys.
/// </summary>
internal sealed class GuidV7ValueGenerator : ValueGenerator<Guid>
{
    /// <summary>
    /// Generates a new GUIDv7 for the current entity entry.
    /// </summary>
    public override Guid Next(EntityEntry entry) => Guid.CreateVersion7();

    /// <summary>
    /// GUIDv7 is final and should not be treated as a temporary value.
    /// </summary>
    public override bool GeneratesTemporaryValues => false;
}
