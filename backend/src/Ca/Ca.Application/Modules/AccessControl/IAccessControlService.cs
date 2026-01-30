using Ca.Application.Modules.AccessControl.Commands;
using Ca.Contracts.Responses.AccessControl;
using Ca.Shared.Results;

namespace Ca.Application.Modules.AccessControl;

public interface IAccessControlService
{
    public Task<OperationResult<AccessRoleResponse>> UpsertRoleAsync(AccessRoleCommand command);

    public Task<OperationResult<AccessRoleResponse>> SeedSuperAdminRoleAndPermissionsAsync(AccessRoleCommand command);
}