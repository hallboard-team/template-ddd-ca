namespace Ca.Infrastructure.Persistence.EFCore.Common.Conventions.Filters;

/// <summary>
/// Canonical names for EF Core 10 named global query filters.
/// Keep these centralized so filters can be selectively disabled safely.
/// </summary>
internal static class QueryFilterNames
{
    internal const string Tenant = "Tenant";
    internal const string SoftDelete = "SoftDelete";
}
