using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common;

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
    ///             <paramref name="bypassQueryFilters" />
    ///         </term>
    ///         <description>
    ///             If <c>true</c>, applies
    ///             <see cref="EntityFrameworkQueryableExtensions.IgnoreQueryFilters{TEntity}(IQueryable{TEntity})" />
    ///             to bypass global filters (including tenant filter).
    ///             Important: when enabled, tenant isolation filters are disabled for this query.
    ///             EF Core bypasses all global filters, not only tenant.
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
    ///     .ForUpdate(bypassQueryFilters: true, useSplitQuery: true, reason: "EditProjectCommand")
    ///     .FirstOrDefaultAsync(p => p.Id == id, ct);
    /// 
    /// project.UpdateTitle("New Title");
    /// await _context.SaveChangesAsync();
    /// </code>
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="bypassQueryFilters">
    /// Whether to bypass EF Core global query filters for this query.
    /// Warning: this disables tenant query filters too.
    /// </param>
    /// <param name="useSplitQuery">Whether to split queries for large include graphs.</param>
    /// <param name="reason">Optional SQL tag for debugging/profiling purposes.</param>
    /// <returns>The modified query with tracking and any requested behaviors applied.</returns>
    public static IQueryable<T> ForUpdate<T>(
        this IQueryable<T> query,
        bool bypassQueryFilters = false,
        bool useSplitQuery = false,
        string? reason = null
    ) where T : class
    {
        IQueryable<T> result = query.AsTracking().
            TagWith($"UPDATE QUERY{(string.IsNullOrWhiteSpace(reason) ? "" : $": {reason}")}");

        if (bypassQueryFilters)
            result = result.IgnoreQueryFilters();

        if (useSplitQuery)
            result = result.AsSplitQuery();

        return result;
    }
}
