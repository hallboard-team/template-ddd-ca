using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Auth;
using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Results;
using Ca.Domain.Modules.Auth.ValueObjects;

namespace Ca.Infrastructure.Modules.Auth.Postgres;

public class AuthRepositoryPostgres : IAuthRepository
{
    public Task<RegisterResult> CreateAppUserAsync(AppUser appUser, AccessRoleType roleType) =>
        throw new NotImplementedException();

    public Task<LoginResult> LoginAsync(Login login) => throw new NotImplementedException();

    public Task<LoginResult> LoginByEmailAsync(Login login) => throw new NotImplementedException();

    public Task<RegisterResult> SeedSuperAdminAppUserAsync(AppUser appUser, AccessRoleType roleType) =>
        throw new NotImplementedException();
}