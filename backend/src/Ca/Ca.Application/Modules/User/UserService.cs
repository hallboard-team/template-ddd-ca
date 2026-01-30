using Ca.Application.Modules.User.Commands;
using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.User;
using Ca.Shared.Results;

namespace Ca.Application.Modules.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<OperationResult> ChangeUserNameAsync(ChangeNameCommand command, CancellationToken ct)
    {
        AppUser? user = await userRepository.GetUserByIdAsync(command.IdStr, ct);

        if (user is null)
            return new OperationResult(
                IsSuccess: false, new CustomError(ResultErrorCode.NetIdentityFailed, ".Net error")
            );

        user?.ChangeFirstName(command.NewName);

        return new OperationResult(IsSuccess: true, Error: null);
    }

    public async Task<AppUser> GetUserByIdAsync(string idStr, CancellationToken ct) =>
        throw new NotImplementedException();
}