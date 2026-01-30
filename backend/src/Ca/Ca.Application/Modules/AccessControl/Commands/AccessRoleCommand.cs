namespace Ca.Application.Modules.AccessControl.Commands;

public record AccessRoleCommand(
    string RoleName,
    IEnumerable<string> DesiredPermissions
);