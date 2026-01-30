using Microsoft.EntityFrameworkCore;

namespace Ca.Infrastructure.Persistence.EFCore.Common;

public static class EfUpdateExtensions
{
    /// <summary>
    ///     Converts a read-optimized EF Core query (typically configured with global
    ///     <see cref="QueryTrackingBehavior.NoTracking" />)
    ///     into a write-intended query by enabling change tracking and applying optional behaviors.
    ///     Use this method when you intend to load an aggregate for modification so EF Core will track changes and persist
    ///     them on <see cref="DbContext.SaveChanges()" /> without requiring manual <c>Attach</c> or <c>Update</c> calls.
    ///     Optional parameters:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>
    ///                 <paramref name="ignoreSoftDelete" />
    ///             </term>
    ///             <description>
    ///                 If <c>true</c>, disables global query filters (e.g., soft-delete filters) so that entities normally
    ///                 excluded
    ///                 can still be loaded for editing or restoration.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>
    ///                 <paramref name="useSplitQuery" />
    ///             </term>
    ///             <description>
    ///                 If <c>true</c>, uses
    ///                 <see cref="EntityFrameworkQueryableExtensions.AsSplitQuery{TEntity}(IQueryable{TEntity})" />
    ///                 for large or complex include graphs to avoid Cartesian explosion by splitting into multiple SQL
    ///                 queries.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>
    ///                 <paramref name="reason" />
    ///             </term>
    ///             <description>
    ///                 An optional string used with EF Core's
    ///                 <see cref="EntityFrameworkQueryableExtensions.TagWith{TEntity}(IQueryable{TEntity}, string)" />
    ///                 to annotate the generated SQL for easier debugging and profiling (e.g., <c>"EditProjectCommand"</c>).
    ///             </description>
    ///         </item>
    ///     </list>
    ///     Example usage:
    ///     <code>
    /// var project = await _context.Projects
    ///     .ForUpdate(ignoreSoftDelete: false, useSplitQuery: true, reason: "EditProjectCommand")
    ///     .FirstOrDefaultAsync(p => p.Id == id, ct);
    /// 
    /// project.UpdateTitle("New Title");
    /// await _context.SaveChangesAsync();
    /// </code>
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="ignoreSoftDelete">Whether to bypass global query filters (e.g., soft delete).</param>
    /// <param name="useSplitQuery">Whether to split queries for large include graphs.</param>
    /// <param name="reason">Optional SQL tag for debugging/profiling purposes.</param>
    /// <returns>The modified query with tracking and any requested behaviors applied.</returns>
    public static IQueryable<T> ForUpdate<T>(
        this IQueryable<T> query,
        bool ignoreSoftDelete = false,
        bool useSplitQuery = false,
        string? reason = null
    ) where T : class
    {
        IQueryable<T> result = query.AsTracking().
            TagWith($"UPDATE QUERY{(string.IsNullOrWhiteSpace(reason) ? "" : $": {reason}")}");

        // Requires soft-delete implementations on the entity
        if (ignoreSoftDelete)
            result = result.IgnoreQueryFilters();

        if (useSplitQuery)
            result = result.AsSplitQuery();

        return result;
    }
}