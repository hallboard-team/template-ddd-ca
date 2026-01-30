namespace Ca.Application.Modules.Auth.Interfaces;

public interface ITokenService
{
    Task<string?> GetActualUserIdAsync(string? userIdHashed, CancellationToken cancellationToken);

    // public Task<TokenDto> GenerateTokensAsync(
    //     RefreshTokenRequest refreshTokenRequest, AppUser appUser, CancellationToken cancellationToken
    // );
}