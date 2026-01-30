using Ca.Domain.Modules.AccessControl.Entities;
using Ca.Domain.Modules.AccessControl.Enums;

namespace Ca.Domain.Modules.AccessControl.Constants;

public static class AppPermissionsMetadata
{
    public static readonly List<AppPermission> All =
    [
        new()
        {
            AccessPermissionType = AccessPermissionType.AccessSuperAdminPanel,
            Description = "Absolute system access, emergency recovery, all permissions are locked"
        },
        new()
        {
            AccessPermissionType = AccessPermissionType.AccessAdminPanel,
            Description = "Allows access to admin interface"
        },
        new() { AccessPermissionType = AccessPermissionType.BanUser, Description = "Allows banning users" },
        new() { AccessPermissionType = AccessPermissionType.DeleteUser, Description = "Allows deleting user accounts" },
        new()
        {
            AccessPermissionType = AccessPermissionType.AccessParkingPanel,
            Description = "Allows access to parking interface"
        },
        new() { AccessPermissionType = AccessPermissionType.EditOwnProfile, Description = "Edit your own profile" },
        new()
        {
            AccessPermissionType = AccessPermissionType.ApprovePhotos, Description = "Approve or reject uploaded photos"
        },
        new() { AccessPermissionType = AccessPermissionType.SendMessages, Description = "Send private messages" }
    ];
}