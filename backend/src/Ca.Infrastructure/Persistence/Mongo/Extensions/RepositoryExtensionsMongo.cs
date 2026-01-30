using Ca.Domain.Modules.AccessControl;
using Ca.Domain.Modules.Auth;
using Ca.Domain.Modules.Message;
using Ca.Domain.Modules.User;
using Ca.Infrastructure.Modules.AccessControl.Mongo;
using Ca.Infrastructure.Modules.Auth.Mongo;
using Ca.Infrastructure.Modules.Message.Mongo;
using Ca.Infrastructure.Modules.User.Mongo;
using Microsoft.Extensions.DependencyInjection;

namespace Ca.Infrastructure.Persistence.Mongo.Extensions;

public static class RepositoryExtensionsMongo
{
    public static IServiceCollection AddRepositoriesMongo(this IServiceCollection services)
    {
        services.AddScoped<IAccessControlRepository, AccessControlRepositoryMongo>();
        services.AddScoped<IAuthRepository, AuthRepositoryMongo>();
        services.AddScoped<IMessageRepository, MessageRepositoryMongo>();
        services.AddScoped<IUserRepository, UserRepositoryMongo>();

        return services;
    }
}