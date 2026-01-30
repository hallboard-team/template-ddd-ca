using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Enums;

namespace Ca.Domain.Modules.Auth.Results;

public record RegisterResult(
    bool Succeeded,
    AppUser? AppUser,
    AuthUserCreationErrorType ErrorType,
    string? ErrorMessage
);