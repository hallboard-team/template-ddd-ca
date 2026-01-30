using Ca.Domain.Modules.AccessControl.Enums;
using MadEyeMatt.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;

namespace Ca.Infrastructure.Modules.AccessControl.Mongo.Models;

public class AppRoleMongo : MongoIdentityRole<ObjectId>
{
    public IEnumerable<AccessPermissionType> Permissions { get; init; } = [];
    public bool IsLocked { get; init; }
}