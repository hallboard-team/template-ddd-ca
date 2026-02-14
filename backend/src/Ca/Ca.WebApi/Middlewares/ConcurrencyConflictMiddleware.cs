using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ca.WebApi.Middlewares;

/// <summary>
/// Converts EF Core optimistic concurrency exceptions into a consistent HTTP 409 Problem Details response.
/// This middleware handles "stale write" conflicts (for example, row-version/xmin mismatches),
/// not request idempotency concerns.
/// </summary>
public sealed class ConcurrencyConflictMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ConcurrencyConflictMiddleware> _logger;

    public ConcurrencyConflictMiddleware(RequestDelegate next, ILogger<ConcurrencyConflictMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executes the next middleware and maps <see cref="DbUpdateConcurrencyException"/> to HTTP 409.
    /// </summary>
    /// <remarks>
    /// If the response has already started, the exception is rethrown because headers/status can no longer be changed.
    /// </remarks>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Warning level is intentional: this is an expected business race, not a server crash.
            _logger.LogWarning(ex, "Concurrency conflict.");

            if (context.Response.HasStarted)
                // Cannot safely rewrite status/body once response bytes are already sent.
                throw;

            // Replace any partially written state with a standard Problem Details payload.
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/problem+json";

            // 409 tells clients to refresh state and retry with latest data.
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Concurrency conflict",
                Detail = "The resource was modified by another process. Please reload and retry.",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
