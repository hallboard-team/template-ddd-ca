namespace Ca.Application.Modules.Auth.Commands;

public record RegisterSuperAdminCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string RoleNameRaw,
    string Password
);