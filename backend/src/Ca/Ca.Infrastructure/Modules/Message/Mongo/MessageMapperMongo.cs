using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Infrastructure.Modules.Auth.Mongo.Models;

namespace Ca.Infrastructure.Modules.Message.Mongo;

internal static class MessageMapperMongo
{
    internal static AppUserMongo MapAppUserToMongoAppUser(AppUser appUser) =>
        new();
}