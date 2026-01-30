using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Enums;

namespace Ca.Domain.Modules.Auth.Results;

public record LoginResult(
    bool Succeeded,
    AppUser? AppUser,
    AuthLoginErrorType ErrorType,
    string? ErrorMessage
);