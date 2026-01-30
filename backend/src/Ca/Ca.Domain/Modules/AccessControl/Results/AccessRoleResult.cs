using Ca.Domain.Modules.AccessControl.Aggregates;
using Ca.Domain.Modules.AccessControl.Enums;

namespace Ca.Domain.Modules.AccessControl.Results;

public record AccessRoleResult(
    bool Succeeded,
    AppRole? AppRole,
    AccessRoleErrorType ErrorType,
    string? ErrorMessage
);