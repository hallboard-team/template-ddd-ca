using Ca.Application.Modules.AccessControl.Commands;
using Ca.Contracts.Requests.AccessControl;

namespace Ca.WebApi.Modules.AccessControl;

internal static class AccessControlRequestMapper
{
    internal static AccessRoleCommand MapRoleCreationReqToRoleCreationCom(
        AccessRoleRequest request
    ) =>
        new(request.RoleName, request.DesiredPermissions);
}