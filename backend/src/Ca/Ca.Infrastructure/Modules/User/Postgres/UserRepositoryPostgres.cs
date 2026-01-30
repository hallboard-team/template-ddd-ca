using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.User;

namespace Ca.Infrastructure.Modules.User.Postgres;

public class UserRepositoryPostgres : IUserRepository
{
    public Task<AppUser?> GetUserByIdAsync(string idStr, CancellationToken ct) => throw new NotImplementedException();

    public Task<bool> ChangeFirstNameAsync(string idStr, string newFirstName, CancellationToken ct) =>
        throw new NotImplementedException();
}