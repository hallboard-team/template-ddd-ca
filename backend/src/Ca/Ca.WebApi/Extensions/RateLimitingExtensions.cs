using System.Threading.RateLimiting;

namespace Ca.WebApi.Extensions;

internal static class RateLimitingExtensions
{
    internal static IServiceCollection AddRateLimitingService(this IServiceCollection services)
    {
        services.AddRateLimiter(
            options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                    // Sliding Window Policy
                    PartitionedRateLimiter.Create<HttpContext, string>(
                        httpContext =>
                        {
                            string userIdHashedOrIpAddress = httpContext.User.Identity?.IsAuthenticated == true
                                ? httpContext.User.GetHashedUserId()
                                    ?? throw new ArgumentNullException(nameof(userIdHashedOrIpAddress))
                                : httpContext.Connection.RemoteIpAddress?.ToString()
                                    ?? throw new ArgumentNullException(
                                            nameof(httpContext.Connection.RemoteIpAddress)
                                            , "is null while userIdHashed is null too which is unsafe. One of them has to be valid."
                                        );

                            return RateLimitPartition.GetSlidingWindowLimiter(
                                userIdHashedOrIpAddress,
                                _ => new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = 100, // Up to 100 requests allowed
                                    Window = TimeSpan.FromSeconds(20), // Sliding window of 5 minutes
                                    SegmentsPerWindow = 5, // Smooth enforcement (1 segment per minute)
                                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                    QueueLimit = 10 // Allow up to 10 queued requests
                                }
                            );
                        }
                    ),
                    // Concurrent Policy
                    PartitionedRateLimiter.Create<HttpContext, string>(
                        httpContext =>
                        {
                            string userIdHashedOrIpAddress = httpContext.User.Identity?.IsAuthenticated == true
                                ? httpContext.User.GetHashedUserId()
                                    ?? throw new ArgumentNullException(nameof(userIdHashedOrIpAddress))
                                : httpContext.Connection.RemoteIpAddress?.ToString()
                                    ?? throw new ArgumentNullException(
                                            nameof(httpContext.Connection.RemoteIpAddress)
                                            , "is null while userIdHashed is null too which is unsafe. One of them has to be valid."
                                        );

                            return RateLimitPartition.GetConcurrencyLimiter(
                                userIdHashedOrIpAddress,
                                _ => new ConcurrencyLimiterOptions
                                {
                                    PermitLimit = 5, // Up to 5 requests allowed
                                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                    QueueLimit = 2 // Allow up to 2 queued requests        
                                }
                            );
                        }
                    )
                );

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            }
        );

        return services;
    }
}
