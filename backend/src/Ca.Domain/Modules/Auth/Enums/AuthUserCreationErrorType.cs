namespace Ca.Domain.Modules.Auth.Enums;

public enum AuthUserCreationErrorType
{
    EmailAlreadyExists,
    UsernameAlreadyExists,
    AddRoleFailed,
    Unknown,
    None
}