using Ca.Domain.Modules.Auth;
using Ca.Domain.Modules.Message;
using Ca.Domain.Modules.User;
using Ca.Infrastructure.Modules.Auth.Postgres;
using Ca.Infrastructure.Modules.Message.Postgres;
using Ca.Infrastructure.Modules.User.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Extensions;

public static class RepositoryExtensionsPostgres
{
    public static IServiceCollection AddRepositoriesPostgres(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepositoryPostgres>();
        services.AddScoped<IMessageRepository, MessageRepositoryPostgres>();
        services.AddScoped<IUserRepository, UserRepositoryPostgres>();

        return services;
    }
}