namespace Ca.Contracts.Responses.AccessControl;

public record AccessRoleResponse(
    string RoleName,
    IEnumerable<string> UpdatedPermissions
);