using Ca.Application.Modules.User.Commands;
using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Shared.Results;

namespace Ca.Application.Modules.User;

public interface IUserService
{
    internal Task<AppUser> GetUserByIdAsync(string idStr, CancellationToken ct);
    public Task<OperationResult> ChangeUserNameAsync(ChangeNameCommand command, CancellationToken ct);
}