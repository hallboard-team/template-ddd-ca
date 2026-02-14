namespace Ca.WebApi.Extensions;

internal static class PolicyServiceExtensions
{
    internal static IServiceCollection AddPolicyService(this IServiceCollection services)
    {
        // Add authorization policies for roles
        // services.AddAuthorizationBuilder().AddPolicy(
        //     name: AppVariablesExtensions.RequiredAdminRole,
        //     configurePolicy: policy => policy.RequireRole(EnumExtensions.GetRoleStrValue(RoleName.SuperAdmin))
        // ).AddPolicy(
        //     name: AppVariablesExtensions.RequiredModeratorRole,
        //     configurePolicy: policy => policy.RequireRole(
        //         EnumExtensions.GetRoleStrValue(RoleName.SuperAdmin), EnumExtensions.GetRoleStrValue(RoleName.Moderator)
        //     )
        // );

        return services;
    }
}
