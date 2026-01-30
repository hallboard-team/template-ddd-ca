using Ca.Application.Modules.AccessControl.Commands;
using Ca.Contracts.Responses.AccessControl;
using Ca.Domain.Modules.AccessControl;
using Ca.Domain.Modules.AccessControl.Aggregates;
using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.AccessControl.Results;
using Ca.Domain.Modules.Common.Exceptions;
using Ca.Shared.Results;

namespace Ca.Application.Modules.AccessControl;

public class AccessControlService(
    IAccessControlRepository accessControlRepository
) : IAccessControlService
{
    public async Task<OperationResult<AccessRoleResponse>> UpsertRoleAsync(AccessRoleCommand command)
    {
        IEnumerable<AccessPermissionType> desiredPermissions =
            ConvertPermissionsStringToEnum(command.DesiredPermissions);

        AccessRoleResult result = await accessControlRepository.GetAppRoleByNameAsync(command.RoleName);

        if (result.AppRole is null)
        {
            var appRole = AppRole.Create(command.RoleName, desiredPermissions);
            result = await accessControlRepository.CreateAppRoleAsync(appRole);
        }
        else
        {
            result.AppRole.ReplacePermissions(desiredPermissions);
            result = await accessControlRepository.UpdatePermissionsAsync(result.AppRole);
        }

        return AccessControlMapper.MapRoleResultToRoleResponse(result);
    }

    /// <summary>
    ///     Seed SuperAdmin role and permissions on app startup.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<OperationResult<AccessRoleResponse>> SeedSuperAdminRoleAndPermissionsAsync(
        AccessRoleCommand command
    )
    {
        var appRole = AppRole.CreateSuperAdmin(command.RoleName, AccessPermissionType.AccessSuperAdminPanel);

        AccessRoleResult result = await accessControlRepository.CreateAppRoleAsync(appRole);

        return AccessControlMapper.MapRoleResultToRoleResponse(result);
    }


    /// <summary>
    ///     Converts permissions string to permissions enum of AccessPermissionType
    /// </summary>
    /// <param name="permissionsStr"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    private IEnumerable<AccessPermissionType> ConvertPermissionsStringToEnum(IEnumerable<string> permissionsStr)
    {
        List<AccessPermissionType> permissionsEnum = [];
        foreach (string newPermission in permissionsStr)
        {
            AccessPermissionType type = Enum.TryParse(newPermission, ignoreCase: true, out AccessPermissionType parsed)
                ? parsed
                : throw new DomainException($"Invalid permission: {newPermission}");

            permissionsEnum.Add(type);
        }

        return permissionsEnum;
    }
}