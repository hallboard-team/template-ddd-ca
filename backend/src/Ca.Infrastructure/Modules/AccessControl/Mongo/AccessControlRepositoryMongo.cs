using System.Security.Claims;
using Ca.Domain.Modules.AccessControl;
using Ca.Domain.Modules.AccessControl.Aggregates;
using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.AccessControl.Results;
using Ca.Domain.Shared;
using Ca.Infrastructure.Modules.AccessControl.Mongo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Ca.Infrastructure.Modules.AccessControl.Mongo;

public class AccessControlRepositoryMongo(
    RoleManager<AppRoleMongo> roleManager,
    ILogger<AccessControlRepositoryMongo> logger
) : IAccessControlRepository
{
    /// <summary>
    ///     Create a new role and assign permissions to it.
    /// </summary>
    /// <param name="appRole"></param>
    /// <returns>AccessRoleResult</returns>
    /// <exception cref="ApplicationException"></exception>
    public async Task<AccessRoleResult> CreateAppRoleAsync(AppRole appRole)
    {
        var appRoleMongo = new AppRoleMongo { Name = appRole.Name.Value };

        IdentityResult result = await roleManager.CreateAsync(appRoleMongo);
        if (!result.Succeeded)
        {
            string? roleError = result.Errors.FirstOrDefault()?.Description;
            logger.LogError(roleError);
            return new AccessRoleResult(Succeeded: false, AppRole: null, AccessRoleErrorType.AddRoleFailed, roleError);
        }


        return new AccessRoleResult(Succeeded: true, appRole, AccessRoleErrorType.None, ErrorMessage: null);
    }

    /// <summary>
    ///     Get AppRole by name or return null
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<AccessRoleResult> GetAppRoleByNameAsync(string roleName)
    {
        AppRoleMongo? appMongoRole = await roleManager.FindByNameAsync(roleName);

        if (appMongoRole is null)
        {
            return new AccessRoleResult(
                Succeeded: false,
                AppRole: null,
                AccessRoleErrorType.AppRoleDoesNotExist,
                $"$App role with name {roleName}  does not exist"
            );
        }

        AppRole appRole = AccessControlMapperMongo.MapAppRoleMongoToAppRole(appMongoRole);
        return new AccessRoleResult(Succeeded: true, appRole, AccessRoleErrorType.None, ErrorMessage: null);
    }

    /// <summary>
    ///     Update permissions/claims of a role.
    /// </summary>
    /// <param name="domainRole"></param>
    /// <returns>AccessRoleResult</returns>
    public async Task<AccessRoleResult> UpdatePermissionsAsync(AppRole domainRole)
    {
        AppRoleMongo? existingAppRoleMongo = await roleManager.FindByNameAsync(domainRole.Name.Value);
        if (existingAppRoleMongo is null)
        {
            return new AccessRoleResult(
                Succeeded: false,
                AppRole: null,
                AccessRoleErrorType.AppRoleDoesNotExist,
                $"$App role with name {domainRole.Name}  does not exist"
            );
        }

        IList<Claim>? existingClaims = await roleManager.GetClaimsAsync(existingAppRoleMongo);

        List<Claim> desiredClaims = domainRole.Permissions.
            Select(p => new Claim(CustomVariableNames.Permission, p.Value.ToString())).ToList();

        // Add missing
        foreach (Claim desired in desiredClaims)
            if (!existingClaims.Any(c => c.Type == desired.Type && c.Value == desired.Value))
            {
                IdentityResult? result = await roleManager.AddClaimAsync(existingAppRoleMongo, desired);
                if (!result.Succeeded)
                {
                    return new AccessRoleResult(
                        Succeeded: false,
                        AppRole: null,
                        AccessRoleErrorType.UpdateRolePermissionsFailed,
                        $"Adding claims failed with error, '{result.Errors.FirstOrDefault()?.Description}'"
                    );
                }
            }

        // Remove extras
        foreach (Claim existing in existingClaims)
            if (existing.Type == CustomVariableNames.Permission && desiredClaims.All(c => c.Value != existing.Value))
            {
                IdentityResult result = await roleManager.RemoveClaimAsync(existingAppRoleMongo, existing);
                if (!result.Succeeded)
                {
                    return new AccessRoleResult(
                        Succeeded: false,
                        AppRole: null,
                        AccessRoleErrorType.UpdateRolePermissionsFailed,
                        $"Removing claims failed with error, '{result.Errors.FirstOrDefault()?.Description}'"
                    );
                }
            }

        //TODO: Update AppRole's claims in each loop in C# rather than getting AppRole from DB
        return await GetAppRoleByNameAsync(domainRole.Name.Value);
    }
}