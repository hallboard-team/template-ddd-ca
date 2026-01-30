using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Auth;
using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Enums;
using Ca.Domain.Modules.Auth.Results;
using Ca.Domain.Modules.Auth.ValueObjects;
using Ca.Infrastructure.Modules.Auth.Mongo.Models;
using Ca.Infrastructure.Modules.Common.Mongo;
using Ca.Infrastructure.Persistence.Mongo.Settings;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Ca.Infrastructure.Modules.Auth.Mongo;

public class AuthRepositoryMongo : IAuthRepository
{
    #region CRUD

    /// <summary>
    ///     Seed SuperAdmin AppUser with SuperAdmin role.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="roleType"></param>
    /// <returns>AuthUserCreationResult</returns>
    public async Task<RegisterResult> SeedSuperAdminAppUserAsync(AppUser appUser, AccessRoleType roleType) =>
        await ImplementCreateUserAppAsync(appUser, roleType);

    /// <summary>
    ///     Create a new user with a default client role.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="roleType"></param>
    /// <returns>AuthUserCreationResult</returns>
    public async Task<RegisterResult> CreateAppUserAsync(AppUser appUser, AccessRoleType roleType) =>
        await ImplementCreateUserAppAsync(appUser, roleType);

    public async Task<LoginResult> LoginAsync(Login login)
    {
        // if (!await ValidateRecaptcha(userInput.RecaptchaToken, cancellationToken))
        // {
        //     return new OperationResult<LoginResult>(
        //         false,
        //         Error: new CustomError(
        //             ErrorCode.IsRecaptchaTokenInvalid,
        //             RecaptchaErrorMessage
        //         )
        //     );
        // }

        AppUserMongo? appUserMongo = await CheckCredentials(login);
        if (appUserMongo is null)
        {
            return new LoginResult(
                Succeeded: false,
                AppUser: null,
                AuthLoginErrorType.WrongCredentials,
                "Wrong username or password"
            );
        }

        // if (!await _userManager.IsEmailConfirmedAsync(appUserMongo))
        // {
        //     if (!await SendVerificationCode(appUserMongo, cancellationToken))
        //         throw new ArgumentException(nameof(appUserMongo.UserName) + ": Failed to email the verification code.");
        //
        //     return new OperationResult<LoginResult>(
        //         false,
        //         new LoginResult(
        //             new LoggedInDto(Email: appUserMongo.Email?.ToLower(), IsEmailNotConfirmed: true)
        //         ),
        //         new CustomError(
        //             ErrorCode.IsEmailNotConfirmed
        //         )
        //     );
        // }

        // RefreshTokenRequest refreshTokenRequest = new()
        // {
        //     JtiValue = Guid.CreateVersion7().ToString(),
        //     SessionMetadata = sessionMetadata
        // };
        //
        // return new OperationResult<LoginResult>(
        //     true,
        //     new LoginResult(
        //         Mappers.ConvertAppUserToLoggedInDto(
        //             appUserMongo, await _userManager.GetRolesAsync(appUserMongo), GetMainPhoto(appUserMongo)
        //         ),
        //         await _tokenService.GenerateTokensAsync(refreshTokenRequest, appUserMongo, cancellationToken)
        //     ),
        //     null
        // );

        return new LoginResult(
            Succeeded: true,
            CommonMapperMongo.MapMongoAppUserToAppUser(appUserMongo),
            AuthLoginErrorType.None,
            ErrorMessage: null
        );
        ;
    }

    private async Task<AppUserMongo?> CheckCredentials(Login login)
    {
        AppUserMongo? appUserMongo = login.IsEmail
            ? await _userManager.FindByEmailAsync(login.Email?.Value)
            : await _userManager.FindByNameAsync(login.UserName?.Value);

        if (appUserMongo is null)
            return null;

        if (!await _userManager.CheckPasswordAsync(appUserMongo, login.Password.Value))
            return null;

        return appUserMongo;
    }

    public Task<LoginResult> LoginByEmailAsync(Login login) => throw new NotImplementedException();

    /// <summary>
    ///     The implementation of CreateAppUserAsync with a given role.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="roleType"></param>
    /// <returns>RegisterResult</returns>
    private async Task<RegisterResult> ImplementCreateUserAppAsync(AppUser appUser, AccessRoleType roleType)
    {
        // Check before creation for performance since username/email are checked in _userManager.CreateAppUserAsync 
        AppUserMongo? existingUser = await _userManager.FindByEmailAsync(appUser.Email?.Value);
        if (existingUser != null)
        {
            return new RegisterResult(
                Succeeded: false, AppUser: null, AuthUserCreationErrorType.EmailAlreadyExists, "Email already exists."
            );
        }

        AppUserMongo appUserMongo = CommonMapperMongo.MapAppUserToAppUserMongo(appUser);

        IdentityResult identityResult = await _userManager.CreateAsync(appUserMongo, appUser.Password?.Value);
        if (!identityResult.Succeeded)
        {
            List<string> errors = identityResult.Errors.Select(e => e.Description).ToList();

            if (errors.Any(e => e.Contains("is already taken", StringComparison.OrdinalIgnoreCase)))
            {
                if (errors.Any(e => e.Contains("email", StringComparison.OrdinalIgnoreCase)))
                {
                    return new RegisterResult(
                        Succeeded: false, AppUser: null,
                        AuthUserCreationErrorType.EmailAlreadyExists,
                        "Email already exists."
                    );
                }

                if (errors.Any(e => e.Contains("username", StringComparison.OrdinalIgnoreCase)))

                {
                    return new RegisterResult(
                        Succeeded: false,
                        AppUser: null,
                        AuthUserCreationErrorType.UsernameAlreadyExists,
                        "UserName already exists."
                    );
                }
            }

            return new RegisterResult(
                Succeeded: false, AppUser: null, AuthUserCreationErrorType.Unknown, errors[index: 0]
            ); // Failed with other reasons
        }

        bool addRoleSucceeded = await AddRoleToAppUserAsync(appUserMongo, roleType);
        return addRoleSucceeded
            ? new RegisterResult( // Account created successfully.
                Succeeded: true, appUser, AuthUserCreationErrorType.None, ErrorMessage: null
            )
            : new RegisterResult(
                Succeeded: false, AppUser: null, AuthUserCreationErrorType.AddRoleFailed, "Add role failed."
            );
    }

    private async Task<bool> AddRoleToAppUserAsync(AppUserMongo appUserMongo, AccessRoleType roleType)
    {
        IdentityResult roleResult = await _userManager.AddToRoleAsync(appUserMongo, roleType.ToString());
        if (roleResult.Succeeded)
            return true;

        // Failed to add the role. Delete appUser from DB
        await _userManager.DeleteAsync(appUserMongo);
        return false;
    }

    #endregion CRUD

    #region Db Settings

    private readonly IMongoCollection<AppUserMongo> _collectionUsers;
    private readonly IMongoCollection<RefreshTokenMongo> _collectionRefreshTokens;

    // private readonly IRecaptchaService _recaptchaService; // TODO: Move it
    private readonly UserManager<AppUserMongo> _userManager;

    // constructor - dependency injection
    public AuthRepositoryMongo(
        IMongoClient client, IMyMongoDbSettings dbSettings,
        UserManager<AppUserMongo> userManager
    )
    {
        IMongoDatabase dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collectionUsers = dbName.GetCollection<AppUserMongo>(CollectionNamesMongo.Users);
        _collectionRefreshTokens = dbName.GetCollection<RefreshTokenMongo>(CollectionNamesMongo.RefreshTokens);
        _userManager = userManager;
    }

    #endregion
}