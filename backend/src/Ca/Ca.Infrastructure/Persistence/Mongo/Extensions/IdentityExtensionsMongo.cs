using Ca.Infrastructure.Modules.AccessControl.Mongo.Models;
using Ca.Infrastructure.Modules.Auth.Mongo.Models;
using MadEyeMatt.AspNetCore.Identity.MongoDB;
using MadEyeMatt.MongoDB.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Ca.Infrastructure.Persistence.Mongo.Extensions;

public static class IdentityExtensionsMongo
{
    public static IServiceCollection AddIdentityServiceMongo(this IServiceCollection services)
    {
        services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

        // uses Options via OnConfiguring from AppDbContextMongo.cs
        services.AddMongoDbContext<AppDbContextMongo>();

        services.AddIdentityCore<AppUserMongo>(options =>
                {
                    // Unique email
                    options.User.RequireUniqueEmail = true;

                    // Require confirmed email but no account confirmation
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

                    // Token handling
                    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

                    // Password requirements
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;

                    // Lockout configuration
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(minutes: 10);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                }
            ).AddRoles<AppRoleMongo>().AddUserManager<UserManager<AppUserMongo>>().
            AddSignInManager<SignInManager<AppUserMongo>>().AddRoleManager<RoleManager<AppRoleMongo>>().
            AddDefaultTokenProviders().AddMongoDbStores<AppDbContextMongo>();

        return services;
    }
}