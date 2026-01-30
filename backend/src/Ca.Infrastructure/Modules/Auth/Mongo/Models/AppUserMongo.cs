using MadEyeMatt.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;

namespace Ca.Infrastructure.Modules.Auth.Mongo.Models;

public class AppUserMongo : MongoIdentityUser<ObjectId>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}