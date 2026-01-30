using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Infrastructure.Modules.Auth.Mongo.Models;

namespace Ca.Infrastructure.Modules.Common.Mongo;

public static class CommonMapperMongo
{
    internal static AppUserMongo MapAppUserToAppUserMongo(AppUser appUser) =>
        new()
        {
            FirstName = appUser.FirstName.Value,
            LastName = appUser.LastName.Value,
            Email = appUser.Email.Value,
            UserName = appUser.UserName.Value
        };

    internal static AppUser MapMongoAppUserToAppUser(AppUserMongo appUserMongo) =>
        AppUser.Rehydrate(appUserMongo.FirstName, appUserMongo.LastName, appUserMongo.Email, appUserMongo.UserName);
}