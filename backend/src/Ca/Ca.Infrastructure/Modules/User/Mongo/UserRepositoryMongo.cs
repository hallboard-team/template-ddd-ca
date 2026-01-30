using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.User;
using Ca.Infrastructure.Modules.Auth.Mongo.Models;
using Ca.Infrastructure.Modules.Common.Mongo;
using Ca.Infrastructure.Persistence.Mongo.Settings;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ca.Infrastructure.Modules.User.Mongo;

public class UserRepositoryMongo : IUserRepository
{
    #region Db and Token Settings

    private readonly IMongoCollection<AppUserMongo> _collectionUsers;

    // constructor - dependency injection
    public UserRepositoryMongo(
        IMongoClient client, IMyMongoDbSettings dbSettings,
        UserManager<AppUserMongo> userManager
    )
    {
        IMongoDatabase dbName = client.GetDatabase(dbSettings.DatabaseName)
                                ?? throw new ArgumentNullException(nameof(dbName), "The database name cannot be null.");
        _collectionUsers = dbName.GetCollection<AppUserMongo>(CollectionNamesMongo.Users);
    }

    #endregion

    #region CRUD

    public async Task<AppUser?> GetUserByIdAsync(string idStr, CancellationToken ct)
    {
        ObjectId userId = ObjectIdHelperMongo.ConvertStringToObjectId(idStr);

        AppUserMongo appUserMongo = await _collectionUsers.Find(appUser => appUser.Id == userId).
            FirstOrDefaultAsync(ct);

        return CommonMapperMongo.MapMongoAppUserToAppUser(appUserMongo);
    }

    public async Task<bool> ChangeFirstNameAsync(string idStr, string newFirstName, CancellationToken ct)
    {
        ObjectId userId = ObjectIdHelperMongo.ConvertStringToObjectId(idStr);

        UpdateDefinition<AppUserMongo> updateDefinition = Builders<AppUserMongo>.Update.Set(
            appUser => appUser.FirstName, newFirstName
        );

        UpdateResult result = await _collectionUsers.UpdateOneAsync(
            appUser => appUser.Id == userId, updateDefinition, options: null, ct
        );

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    #endregion CRUD
}