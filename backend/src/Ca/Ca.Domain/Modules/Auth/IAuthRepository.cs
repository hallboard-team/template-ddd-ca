using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Results;
using Ca.Domain.Modules.Auth.ValueObjects;

namespace Ca.Domain.Modules.Auth;

public interface IAuthRepository
{
    public Task<RegisterResult> SeedSuperAdminAppUserAsync(AppUser appUser, AccessRoleType roleType);

    public Task<RegisterResult> CreateAppUserAsync(AppUser appUser, AccessRoleType roleType);
    public Task<LoginResult> LoginAsync(Login login);

    // public Task<OperationResult<LoginResult>> VerifyAsync(
    //     VerifyDto verifyDto, SessionMetadata sessionMetadata, CancellationToken cancellationToken
    // );
    //
    // public Task<OperationResult> ResendVerifyCodeAsync(
    //     ResendCodeRequest resendCodeRequest, CancellationToken cancellationToken
    // );
    //
    // public Task<OperationResult<LoginResult>> LoginAsync(
    //     LoginDto loginDto, SessionMetadata sessionMetadata, CancellationToken cancellationToken
    // );
    //
    // public Task<OperationResult<TokenSet>> RefreshTokensAsync(
    //     RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken
    // );
    //
    // public Task<OperationResult<LoggedInSession>> ReloadLoggedInUserAsync(
    //     string userIdHashed, CancellationToken cancellationToken
    // );
    //
    // public Task<OperationResult> RequestResetPasswordAsync(
    //     ResetPasswordRequest request, CancellationToken cancellationToken
    // );
    //
    // public Task<OperationResult> ResetPasswordAsync(ResetPassword resetPassword, CancellationToken cancellationToken);
    // public Task<OperationResult<DeleteResult>> DeleteUserAsync(string? userEmail, CancellationToken cancellationToken);
    //
    // public Task<OperationResult<bool>> UpdateLastActive(
    //     string loggedInUserIdHashed, CancellationToken cancellationToken
    // );
}