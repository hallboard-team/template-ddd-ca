using Ca.Application.Modules.Auth.Commands;
using Ca.Application.Modules.Auth.Interfaces;
using Ca.Contracts.Responses.Auth;
using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Auth;
using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Results;
using Ca.Domain.Modules.Auth.ValueObjects;
using Ca.Shared.Results;

namespace Ca.Application.Modules.Auth;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<OperationResult<RegisterResponse>> SeedSuperAdminAppUserAsync(RegisterSuperAdminCommand command)
    {
        var appUser = AppUser.CreateSuperAdmin( // It guards from creating extra super admins
            command.FirstName, command.LastName, command.Email, command.UserName, command.RoleNameRaw, command.Password
        );

        RegisterResult result = await authRepository.SeedSuperAdminAppUserAsync(
            appUser, AccessRoleType.SuperAdmin
        );

        return AuthMapper.MapRegisterResultToOperationResult(result);
    }

    public async Task<OperationResult<RegisterResponse>> CreateAsync(RegisterCommand command)
    {
        var appUser = AppUser.Create( // Aggregate 
            command.FirstName, command.LastName, command.Email, command.UserName, command.Password
        );

        RegisterResult result = await authRepository.CreateAppUserAsync(appUser, AccessRoleType.Client);

        return AuthMapper.MapRegisterResultToOperationResult(result);
    }

    public async Task<OperationResult<LoginResponse>> LoginAsync(LoginCommand command)
    {
        var login = Login.Create(command.Credential, command.Password);

        // var metadata = SessionMetadata.Create(
        //     command.SessionMetadata.DeviceType, command.SessionMetadata.DeviceName, command.SessionMetadata.UserAgent,
        //     command.SessionMetadata.IpAddress, command.SessionMetadata.Location
        // );

        LoginResult loginResult = await authRepository.LoginAsync(login);

        // if(loginResult.Suceeded)
        //     SessionMetadata

        return AuthMapper.MapAppUserToLoginResult(loginResult);
    }
}