using Ca.Contracts.Responses.Auth;
using Ca.Domain.Modules.Auth.Enums;
using Ca.Domain.Modules.Auth.Results;
using Ca.Shared.Results;

namespace Ca.Application.Modules.Auth;

internal static class AuthMapper
{
    internal static OperationResult<RegisterResponse> MapRegisterResultToOperationResult(RegisterResult result) =>
        result.Succeeded && result.AppUser is not null
            ? new OperationResult<RegisterResponse>(
                IsSuccess: true,
                new RegisterResponse(
                    result.AppUser.FirstName.Value,
                    result.AppUser.LastName.Value,
                    result.AppUser.Email.Value,
                    result.AppUser.UserName.Value
                ),
                Error: null
            )
            : result.ErrorType switch
            {
                AuthUserCreationErrorType.EmailAlreadyExists => new OperationResult<RegisterResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AuthUserCreationErrorType.EmailAlreadyExists, result.ErrorMessage)
                ),
                AuthUserCreationErrorType.UsernameAlreadyExists => new OperationResult<RegisterResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AuthUserCreationErrorType.UsernameAlreadyExists, result.ErrorMessage)
                ),
                AuthUserCreationErrorType.AddRoleFailed => new OperationResult<RegisterResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AuthUserCreationErrorType.AddRoleFailed, result.ErrorMessage)
                ),
                _ => new OperationResult<RegisterResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AuthUserCreationErrorType.Unknown, result.ErrorMessage)
                )
            };

    internal static OperationResult<LoginResponse> MapAppUserToLoginResult(LoginResult result) =>
        result.Succeeded
            ? new OperationResult<LoginResponse>(
                IsSuccess: true,
                new LoginResponse(
                    IsRecaptchaTokenInvalid: false,
                    IsWrongCreds: false,
                    Email: result.AppUser.Email.Value,
                    UserName: result.AppUser.UserName.Value,
                    FirstName: result.AppUser.FirstName.Value,
                    LastName: result.AppUser.LastName.Value
                ),
                Error: null
            )
            : result.ErrorType switch
            {
                AuthLoginErrorType.WrongCredentials => new OperationResult<LoginResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AuthLoginErrorType.WrongCredentials, result.ErrorMessage)
                ),
                _ => new OperationResult<LoginResponse>(
                    IsSuccess: false,
                    Error: new CustomError(AuthLoginErrorType.Unknown, result.ErrorMessage)
                )
            };
}