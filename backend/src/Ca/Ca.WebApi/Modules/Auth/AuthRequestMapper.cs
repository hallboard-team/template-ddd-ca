using Ca.Application.Modules.Auth.Commands;
using Ca.Contracts.Requests.Auth;
using Ca.Domain.Modules.Startup.Entities;

namespace Ca.WebApi.Modules.Auth;

internal static class AuthRequestMapper
{
    internal static RegisterSuperAdminCommand
        MapRegisterSuperAdminRequestToRegisterCommand(SuperAdminSeedInfo request) =>
        new(
            request.FirstName,
            request.LastName,
            request.Email,
            request.UserName,
            request.RoleName,
            request.Password
        );

    internal static RegisterCommand MapRegisterRequestToRegisterCommand(RegisterRequest request) =>
        new(
            request.FirstName,
            request.LastName,
            request.Email,
            request.UserName,
            request.Password
        );

    internal static LoginCommand MapLoginRequestToLoginCommand(LoginRequest request, SessionMetadataDto metadata) =>
        new(
            request.Credential,
            request.Password,
            metadata
        );
}