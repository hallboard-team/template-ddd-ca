using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Infrastructure.Modules.Auth.Mongo.Models;

namespace Ca.Infrastructure.Modules.Auth.Mongo;

internal static class AuthMapperMongo
{
    internal static AppUserMongo MapAppUserToAppUserMongo(AppUser appUser) =>
        new()
        {
            FirstName = appUser.FirstName.Value,
            LastName = appUser.LastName.Value,
            Email = appUser.Email.Value,
            UserName = appUser.UserName.Value
        };
}