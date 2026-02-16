using Ca.Infrastructure.Persistence.EFCore.Common.Conventions.Filters;
using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common.Extensions;

public static class EfUpdateExtensions
{
    /// <summary>
    /// Converts a read-optimized EF Core query (typically configured with global
    /// <see cref="QueryTrackingBehavior.NoTracking" />) into a write-intended query.
    /// Use this when loading an aggregate for modification so EF Core tracks changes and persists
    /// them on <see cref="DbContext.SaveChanges()" /> without manual <c>Attach</c>/<c>Update</c>.
    /// By default this helper keeps global query filters enabled.
    /// Optional parameters:
    /// <list type="bullet">
    ///     <item>
    ///         <term>
    ///             <paramref name="bypassSoftDeleteQueryFilter" />
    ///             and
    ///             <paramref name="bypassTenantQueryFilter" />
    ///         </term>
    ///         <description>
    ///             If <c>true</c>, applies
    ///             <see cref="EntityFrameworkQueryableExtensions.IgnoreQueryFilters{TEntity}(IQueryable{TEntity})" />
    ///             for the named filter(s).
    ///             Important: tenant isolation filters are security-sensitive; only bypass them in
    ///             internal/admin-only paths. Named filters must exist in the model for bypassing to
    ///             have any effect.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <paramref name="useSplitQuery" />
    ///         </term>
    ///         <description>
    ///             If <c>true</c>, uses
    ///             <see cref="EntityFrameworkQueryableExtensions.AsSplitQuery{TEntity}(IQueryable{TEntity})" />
    ///             for larger include graphs.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <paramref name="reason" />
    ///         </term>
    ///         <description>
    ///             Optional SQL tag via
    ///             <see cref="EntityFrameworkQueryableExtensions.TagWith{TEntity}(IQueryable{TEntity}, string)" />
    ///             for profiling/debugging.
    ///         </description>
    ///     </item>
    /// </list>
    ///     Example usage:
    ///     <code>
    /// var project = await _context.Projects
    ///     .ForUpdate(bypassTenantQueryFilter: true, useSplitQuery: true, reason: "EditProjectCommand")
    ///     .FirstOrDefaultAsync(p => p.Id == id, ct);
    /// 
    /// project.UpdateTitle("New Title");
    /// await _context.SaveChangesAsync();
    /// </code>
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="bypassSoftDeleteQueryFilter">
    /// Whether to bypass the named soft-delete query filter for this query.
    /// </param>
    /// <param name="bypassTenantQueryFilter">
    /// Whether to bypass the named tenant query filter for this query.
    /// Warning: tenant isolation is security-sensitive; only bypass in internal/admin paths.
    /// </param>
    /// <param name="useSplitQuery">Whether to split queries for large include graphs.</param>
    /// <param name="reason">Optional SQL tag for debugging/profiling purposes.</param>
    /// <returns>The modified query with tracking and any requested behaviors applied.</returns>
    public static IQueryable<T> ForUpdate<T>(
        this IQueryable<T> query,
        bool bypassSoftDeleteQueryFilter = false,
        bool bypassTenantQueryFilter = false,
        bool useSplitQuery = false,
        string? reason = null
    ) where T : class
    {
        IQueryable<T> result = query.AsTracking().
            TagWith($"UPDATE QUERY{(string.IsNullOrWhiteSpace(reason) ? "" : $": {reason}")}");

        if (bypassSoftDeleteQueryFilter)
            result = result.IgnoreQueryFilters([QueryFilterNames.SoftDelete]);
            
        if (bypassTenantQueryFilter)
            result = result.IgnoreQueryFilters([QueryFilterNames.Tenant]);

        if (useSplitQuery)
            result = result.AsSplitQuery();

        return result;
    }
}
