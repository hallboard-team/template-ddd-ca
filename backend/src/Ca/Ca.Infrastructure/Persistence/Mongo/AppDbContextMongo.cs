using Ca.Infrastructure.Persistence.Mongo.Settings;
using MadEyeMatt.MongoDB.DbContext;
using Microsoft.Extensions.Options;

namespace Ca.Infrastructure.Persistence.Mongo;

public class AppDbContextMongo(
    MongoDbContextOptions<AppDbContextMongo> options,
    IOptions<MyMongoDbSettings> settings
) : MongoDbContext(options)
{
    protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
    {
        MyMongoDbSettings s = settings.Value;
        builder.UseDatabase(s.ConnectionString, s.DatabaseName);
    }
}