using Ca.Application.Modules.AccessControl;
using Ca.Application.Modules.Auth;
using Ca.Application.Modules.Auth.Interfaces;
using Ca.Application.Modules.User;
using Microsoft.Extensions.DependencyInjection;

namespace Ca.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccessControlService, AccessControlService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}