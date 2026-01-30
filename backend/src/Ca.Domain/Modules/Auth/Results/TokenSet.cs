using Ca.Domain.Modules.Auth.Entities;

namespace Ca.Domain.Modules.Auth.Results;

public record TokenSet(
    string AccessToken,
    RefreshToken RefreshToken
);