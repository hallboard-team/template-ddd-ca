using Ca.Contracts.Responses.AccessControl;
using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.AccessControl.Results;
using Ca.Domain.Modules.AccessControl.ValueObjects;
using Ca.Shared.Results;

namespace Ca.Application.Modules.AccessControl;

internal static class AccessControlMapper
{
    internal static OperationResult<AccessRoleResponse> MapRoleResultToRoleResponse(AccessRoleResult result) =>
        result.Succeeded && result.AppRole is not null
            ? new OperationResult<AccessRoleResponse>(
                result.Succeeded,
                new AccessRoleResponse(
                    result.AppRole.Name.Value,
                    ConvertPermissionsToString(result.AppRole.Permissions)
                ),
                Error: null
            )
            : result.ErrorType switch
            {
                AccessRoleErrorType.AppRoleDoesNotExist => new OperationResult<AccessRoleResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AccessRoleErrorType.AppRoleDoesNotExist, result.ErrorMessage)
                ),
                AccessRoleErrorType.AddRoleFailed => new OperationResult<AccessRoleResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AccessRoleErrorType.AddRoleFailed, result.ErrorMessage)
                ),
                AccessRoleErrorType.RemoveRoleFailed => new OperationResult<AccessRoleResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AccessRoleErrorType.UpdateRolePermissionsFailed, result.ErrorMessage)
                ),
                _ => new OperationResult<AccessRoleResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AccessRoleErrorType.Unknown, result.ErrorMessage)
                )
            };

    /// <summary>
    ///     Converts a list of permissions from type Permission to string
    /// </summary>
    /// <param name="permissionsEnum"></param>
    /// <returns>A string list of permissions</returns>
    private static IEnumerable<string> ConvertPermissionsToString(IEnumerable<Permission> permissionsEnum)
    {
        List<string> permissionsStr = [];
        foreach (Permission permission in permissionsEnum) permissionsStr.Add(permission.ToString());

        return permissionsStr;
    }
}