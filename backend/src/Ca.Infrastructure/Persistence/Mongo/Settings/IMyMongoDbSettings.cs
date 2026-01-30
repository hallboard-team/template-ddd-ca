namespace Ca.Infrastructure.Persistence.Mongo.Settings;

public interface IMyMongoDbSettings
{
    string? ConnectionString { get; }
    string? DatabaseName { get; }
}