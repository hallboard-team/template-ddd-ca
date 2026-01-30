using MongoDB.Bson;

namespace Ca.Infrastructure.Modules.Common.Mongo;

internal static class ObjectIdHelperMongo
{
    internal static ObjectId ConvertStringToObjectId(string idStr)
    {
        // Convert string to ObjectId
        bool isSuccess = ObjectId.TryParse(idStr, out ObjectId id);

        // Validate ObjectId
        if (!(isSuccess && ValidateObjectId(id)))
            throw new ArgumentNullException(nameof(id), "Invalid or null ObjectId.");

        return id;
    }

    internal static bool ValidateObjectId(ObjectId? objectId) =>
        objectId.HasValue && !objectId.Equals(ObjectId.Empty);
}