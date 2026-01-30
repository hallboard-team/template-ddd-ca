namespace Ca.Contracts.Requests.AccessControl;

public record AccessRoleRequest(
    string RoleName,
    IEnumerable<string> DesiredPermissions
);