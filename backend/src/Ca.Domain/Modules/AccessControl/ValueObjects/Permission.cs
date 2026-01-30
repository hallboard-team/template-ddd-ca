using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Common.Exceptions;

namespace Ca.Domain.Modules.AccessControl.ValueObjects;

public sealed record
    Permission // Has to be a record so calling .Remove(obj) on HashSet<T> works by value-based equality
{
    private Permission(AccessPermissionType value) => Value = value;

    public AccessPermissionType Value { get; }

    public bool IsAccessSuperAdminPanel => Value == AccessPermissionType.AccessSuperAdminPanel;

    /// <summary>
    ///     // Create Permission without validation when creating a SuperUser for seeding
    /// </summary>
    /// <param name="accessPermissionTypeRaw"></param>
    /// <returns></returns>
    public static Permission CreateUnsafe(AccessPermissionType accessPermissionTypeRaw)
        => new(accessPermissionTypeRaw);

    /// <summary>
    ///     Create Permission with validation to create all permissions except SuperAdmin
    /// </summary>
    /// <param name="accessPermissionTypeRaw"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Permission Create(AccessPermissionType accessPermissionTypeRaw)
    {
        if (accessPermissionTypeRaw == AccessPermissionType.AccessSuperAdminPanel)
            throw new DomainException("Creating SuperAdmin accessPermissionType is not allowed.");

        return new Permission(accessPermissionTypeRaw);
    }
}