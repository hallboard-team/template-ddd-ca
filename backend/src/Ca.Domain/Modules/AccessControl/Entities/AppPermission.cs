using Ca.Domain.Modules.AccessControl.Enums;

namespace Ca.Domain.Modules.AccessControl.Entities;

public class AppPermission
{
    public string Name => AccessPermissionType.ToString(); // Enum.ToString()
    public AccessPermissionType AccessPermissionType { get; init; }
    public string Description { get; init; } = string.Empty;
}