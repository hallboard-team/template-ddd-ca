using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.AccessControl.ValueObjects;
using Ca.Domain.Modules.Auth.ValueObjects;
using Ca.Domain.Modules.Common.Exceptions;

namespace Ca.Domain.Modules.AccessControl.Aggregates;

public class AppRole
{
    private AppRole()
    {
    }

    public Name Name { get; private set; }
    public HashSet<Permission> Permissions { get; private init; } = [];
    public bool IsLocked { get; private set; } // Prevent from changing permissions. SuperAdmin role is always locked.

    /// <summary>
    ///     Create SuperAdmin Role and Permission. It's protected by AccessSuperAdminPanel
    /// </summary>
    /// <param name="superAdminRoleName"></param>
    /// <param name="accessPermissionType"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static AppRole CreateSuperAdmin(
        string superAdminRoleName, AccessPermissionType accessPermissionType
    ) // First seed
    {
        if (accessPermissionType != AccessPermissionType.AccessSuperAdminPanel)
        {
            throw new DomainException(
                "Only creating SuperAdmin role with AccessSuperAdminPanel permission is allowed."
            );
        }

        return new AppRole
        {
            Name = Name.Create(superAdminRoleName),
            Permissions = [Permission.CreateUnsafe(AccessPermissionType.AccessSuperAdminPanel)],
            IsLocked = true
        };
    }

    /// <summary>
    ///     Create user's AppRole. Unprotected
    /// </summary>
    /// <param name="nameRaw"></param>
    /// <param name="permissionsRaw"></param>
    /// <param name="isLocked"></param>
    /// <returns></returns>
    public static AppRole Create(
        string nameRaw, IEnumerable<AccessPermissionType> permissionsRaw, bool isLocked = false
    )
    {
        var name = Name.Create(nameRaw);

        HashSet<Permission> allowedPermissions = []; // Keep permissions unique by HashSet
        foreach (AccessPermissionType permissionRaw in permissionsRaw)
            allowedPermissions.Add(Permission.Create(permissionRaw)); // Prevent from registering a SuperAdmin

        return new AppRole
        {
            Name = name,
            Permissions = allowedPermissions,
            IsLocked = isLocked
        };
    }

    /// <summary>
    ///     Replaces permissions of unlocked AppRole. HashSet prevents duplication.
    /// </summary>
    /// <param name="newPermissions"></param>
    /// <exception cref="DomainException"></exception>
    public void ReplacePermissions(IEnumerable<AccessPermissionType> newPermissions)
    {
        if (IsLocked)
            throw new DomainException("Cannot modify permissions of a locked role.");

        Permissions.Clear(); // Remove old permissions

        foreach (AccessPermissionType permission in newPermissions)
            Permissions.Add(Permission.Create(permission));
    }

    /// <summary>
    ///     Lock an AppRole so its permissions cannot be modified.
    /// </summary>
    public void Lock() => IsLocked = true;

    /// <summary>
    ///     Unlock and AppRole so its permissions can be modified. Except for the SuperAdmin which is protected.
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void Unlock()
    {
        if (Permissions.Any(p => p.IsAccessSuperAdminPanel))
            throw new DomainException("Cannot unlock the SuperAdmin role.");

        IsLocked = false;
    }
}