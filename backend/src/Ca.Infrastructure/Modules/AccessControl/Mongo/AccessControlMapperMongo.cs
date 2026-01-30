using Ca.Domain.Modules.AccessControl.Aggregates;
using Ca.Infrastructure.Modules.AccessControl.Mongo.Models;

namespace Ca.Infrastructure.Modules.AccessControl.Mongo;

public static class AccessControlMapperMongo
{
    internal static AppRoleMongo MapAppRoleToAppRoleMongo(AppRole appUser) =>
        new()
        {
            Name = appUser.Name.Value,
            Permissions = appUser.Permissions.Select(p => p.Value), // Permissions → Enums
            IsLocked = appUser.IsLocked
        };

    internal static AppRole MapAppRoleMongoToAppRole(AppRoleMongo appRoleMongo) =>
        AppRole.Create(appRoleMongo.Name, appRoleMongo.Permissions, appRoleMongo.IsLocked);
}