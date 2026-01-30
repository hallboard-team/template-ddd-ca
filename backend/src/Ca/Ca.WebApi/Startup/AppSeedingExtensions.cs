using Ca.Application.Modules.AccessControl;
using Ca.Application.Modules.AccessControl.Commands;
using Ca.Application.Modules.Auth.Commands;
using Ca.Application.Modules.Auth.Interfaces;
using Ca.Contracts.Responses.AccessControl;
using Ca.Contracts.Responses.Auth;
using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Startup.Entities;
using Ca.Shared.Results;
using Ca.WebApi.Modules.Auth;
using Microsoft.Extensions.Options;

namespace Ca.WebApi.Startup;

internal static class AppSeedingExtensions
{
    internal static async Task PerformAppSeeder(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        SuperAdminSeedInfo superAdminSeedInfo = app.Services.GetRequiredService<IOptions<SuperAdminSeedInfo>>().Value;

        await CreateSuperAdminRole(scope, superAdminSeedInfo);
        await CreateSuperAdmin(scope, superAdminSeedInfo);
        await CreateClientRole(scope);
    }

    private static async Task CreateSuperAdminRole(IServiceScope scope, SuperAdminSeedInfo seedInfo)
    {
        var accessControlService = scope.ServiceProvider.GetRequiredService<IAccessControlService>();

        var command = new AccessRoleCommand(
            seedInfo.RoleName, [AccessPermissionType.AccessSuperAdminPanel.ToString()]
        );

        OperationResult<AccessRoleResponse> result =
            await accessControlService.SeedSuperAdminRoleAndPermissionsAsync(command);
        Console.WriteLine(
            result.IsSuccess
                ? $"{command.RoleName} role created successfully."
                : $"{command.RoleName} role creation failed with error {result.Error?.Message}."
        );
    }

    private static async Task CreateSuperAdmin(IServiceScope scope, SuperAdminSeedInfo seedInfo)
    {
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

        RegisterSuperAdminCommand command = AuthRequestMapper.MapRegisterSuperAdminRequestToRegisterCommand(seedInfo);

        OperationResult<RegisterResponse> registerResult = await authService.SeedSuperAdminAppUserAsync(command);
        Console.WriteLine(
            registerResult.IsSuccess
                ? $"{command.UserName} created successfully."
                : $"{command.UserName} creation failed with error {registerResult.Error?.Message}."
        );
    }

    private static async Task CreateClientRole(IServiceScope scope)
    {
        var accessControlService = scope.ServiceProvider.GetRequiredService<IAccessControlService>();

        var command = new AccessRoleCommand(
            "Client", [
                AccessPermissionType.EditOwnProfile.ToString(),
                AccessPermissionType.SendMessages.ToString()
            ]
        );

        OperationResult<AccessRoleResponse> result = await accessControlService.UpsertRoleAsync(command);
        Console.WriteLine(
            result.IsSuccess
                ? $"{command.RoleName} role created successfully."
                : $"{command.RoleName} role creation failed with error {result.Error?.Message}."
        );
    }
}