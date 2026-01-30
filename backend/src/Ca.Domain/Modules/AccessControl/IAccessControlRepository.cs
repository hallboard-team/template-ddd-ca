using Ca.Domain.Modules.AccessControl.Aggregates;
using Ca.Domain.Modules.AccessControl.Results;

namespace Ca.Domain.Modules.AccessControl;

public interface IAccessControlRepository
{
    public Task<AccessRoleResult> CreateAppRoleAsync(AppRole appRole);

    public Task<AccessRoleResult> GetAppRoleByNameAsync(string roleName);

    public Task<AccessRoleResult> UpdatePermissionsAsync(AppRole domainRole);
}