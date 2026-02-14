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
    ///                 <paramref name="ignoreGlobalFilters" />
    ///             </term>
    ///             <description>
    ///                 If <c>true</c>, disables EF Core global query filters for this query via
    ///                 <see cref="EntityFrameworkQueryableExtensions.IgnoreQueryFilters{TEntity}(IQueryable{TEntity})" />.
    ///                 This is commonly used for soft-delete restoration flows, but it also bypasses any other global
    ///                 filters (for example multi-tenant filters). Use only in trusted paths.
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
    ///     .ForUpdate(ignoreGlobalFilters: false, useSplitQuery: true, reason: "EditProjectCommand")
    ///     .FirstOrDefaultAsync(p => p.Id == id, ct);
    /// 
    /// project.UpdateTitle("New Title");
    /// await _context.SaveChangesAsync();
    /// </code>
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="ignoreGlobalFilters">
    /// Whether to bypass EF Core global query filters for this query (soft-delete and any other configured filters).
    /// </param>
    /// <param name="useSplitQuery">Whether to split queries for large include graphs.</param>
    /// <param name="reason">Optional SQL tag for debugging/profiling purposes.</param>
    /// <returns>The modified query with tracking and any requested behaviors applied.</returns>
    public static IQueryable<T> ForUpdate<T>(
        this IQueryable<T> query,
        bool ignoreGlobalFilters = false,
        bool useSplitQuery = false,
        string? reason = null
    ) where T : class
    {
        IQueryable<T> result = query.AsTracking().
            TagWith($"UPDATE QUERY{(string.IsNullOrWhiteSpace(reason) ? "" : $": {reason}")}");

        // Note: this bypasses all global filters, including soft-delete.
        if (ignoreGlobalFilters)
            result = result.IgnoreQueryFilters();

        if (useSplitQuery)
            result = result.AsSplitQuery();

        return result;
    }
}
