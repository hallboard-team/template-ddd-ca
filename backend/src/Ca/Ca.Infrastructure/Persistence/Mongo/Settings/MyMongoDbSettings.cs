namespace Ca.Infrastructure.Persistence.Mongo.Settings;

public sealed class MyMongoDbSettings : IMyMongoDbSettings
{
    public string? ConnectionString { get; init; }
    public string? DatabaseName { get; init; }
}