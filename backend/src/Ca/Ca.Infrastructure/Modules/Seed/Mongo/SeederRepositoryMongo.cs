// TODO Use the permissions and remove this file

// using System.Security.Claims;
// using Ca.Domain.Modules.Auth.Aggregates;
// using Ca.Domain.Modules.Auth.Enums;
// using Ca.Domain.Modules.Auth.Results;
// using Ca.Domain.Shared;
// using Ca.Infrastructure.Modules.Auth.Mongo.Models;
// using Ca.Infrastructure.Modules.Common.Mongo;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
//
// namespace Ca.Infrastructure.Modules.Seed.Mongo;
//
// public class SeederRepositoryMongo(
//     UserManager<AppUserMongo> userManager,
//     RoleManager<AppRoleMongo> roleManager,
//     ILogger<SeederRepositoryMongo> logger
// )
//     : ISeederRepository
// {
//     public async Task<AuthUserCreationResult> SeedSuperAdminAsync(AppUser appUser)
//     {
//         AppUserMongo? existingSuperAdmin = await userManager.FindByEmailAsync(appUser.Email?.Value);
//         if (existingSuperAdmin != null)
//         {
//             logger.LogWarning("SuperAdmin exists already.");
//             return new AuthUserCreationResult(AppUser: null, ErrorType.EmailAlreadyExists);
//         }
//
//         AppUserMongo appUserMongo = CommonMapperMongo.MapAppUserToAppUserMongo(appUser);
//
//         IdentityResult result = await userManager.CreateAsync(appUserMongo, appUser.Password?.Value);
//
//         if (!result.Succeeded)
//             logger.LogError(result.Errors.FirstOrDefault()?.Description);
//     }
//
//     public async Task SeedRolesAndPermissionsAsync()
//     {
//         await EnsureRoleWithPermissions(
//             AccessRoleType.SuperAdmin, [
//                 AccessPermissionType.AccessSuperAdminPanel
//             ]
//         );
//
//         await EnsureRoleWithPermissions(
//             AccessRoleType.Admin, [
//                 AccessPermissionType.BanUser,
//                 AccessPermissionType.DeleteUser,
//                 AccessPermissionType.AccessAdminPanel
//             ]
//         );
//
//         await EnsureRoleWithPermissions(
//             AccessRoleType.Moderator, [
//                 AccessPermissionType.BanUser,
//                 AccessPermissionType.ApprovePhotos
//             ]
//         );
//
//         await EnsureRoleWithPermissions(
//             AccessRoleType.Parking, [
//                 AccessPermissionType.AccessParkingPanel,
//                 AccessPermissionType.SendMessages,
//                 AccessPermissionType.EditOwnProfile
//             ]
//         );
//
//         await EnsureRoleWithPermissions(
//             AccessRoleType.Client, [
//                 AccessPermissionType.SendMessages,
//                 AccessPermissionType.EditOwnProfile
//             ]
//         );
//     }
//
//     private async Task EnsureRoleWithPermissions(AccessRoleType authRoleEnum, AccessPermissionType[] permissions)
//     {
//         IdentityResult? result = null;
//
//         var roleName = authRoleEnum.ToString();
//         AppRoleMongo role = await roleManager.FindByNameAsync(roleName);
//         if (role is null)
//         {
//             role = new AppRoleMongo { FirstName = roleName };
//             result = await roleManager.CreateAsync(role);
//
//             if (!result.Succeeded)
//                 logger.LogError(result.Errors.FirstOrDefault()?.Description);
//         }
//
//         IList<Claim>? existingClaims = await roleManager.GetClaimsAsync(role);
//
//         // CustomVariableNames.Permission == "accessPermissionType" created for consistency
//         foreach (AccessPermissionType accessPermissionType in permissions)
//             if (!existingClaims.Any(claim => claim.Type == CustomVariableNames.Permission &&
//                                              claim.Value == accessPermissionType.ToString()
//                 ))
//             {
//                 result = await roleManager.AddClaimAsync(
//                     role,
//                     new Claim(CustomVariableNames.Permission, accessPermissionType.ToString())
//                 );
//
//                 if (!result.Succeeded)
//                     logger.LogError(result.Errors.FirstOrDefault()?.Description);
//             }
//     }
// }

