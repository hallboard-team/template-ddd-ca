using System.Security.Claims;

namespace Ca.WebApi.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string? GetHashedUserId(this ClaimsPrincipal user) // principal
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}