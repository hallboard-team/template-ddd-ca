using Microsoft.AspNetCore.Identity;

namespace Ca.Infrastructure.Modules.Auth.Postgres.Models;

public class AppUserPostgres : IdentityUser<Guid>
{
    public required byte[] ConcurrencyToken { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}