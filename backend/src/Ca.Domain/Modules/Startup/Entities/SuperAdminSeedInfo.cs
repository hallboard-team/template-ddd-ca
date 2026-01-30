namespace Ca.Domain.Modules.Startup.Entities;

public class SuperAdminSeedInfo
{
    public required string RoleName { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}