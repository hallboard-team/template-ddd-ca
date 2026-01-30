using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ca.Infrastructure.Modules.Common.Mongo;

public class ApiExceptionMongo
{
    [property: BsonId]
    [property: BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; }
    public DateTime Time { get; set; }
}