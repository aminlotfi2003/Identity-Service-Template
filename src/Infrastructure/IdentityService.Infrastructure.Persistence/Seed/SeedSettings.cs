namespace IdentityService.Infrastructure.Persistence.Seed;

public sealed class SeedSettings
{
    public bool Enable { get; set; } = true;
    public string AdminEmail { get; set; } = "admin@local";
    public string AdminUserName { get; set; } = "admin";
    public string AdminPassword { get; set; } = "";
    public string AdminRoleName { get; set; } = "Admin";
    public string[] DefaultRoles { get; set; } = ["Reader", "Contributor", "Approver", "Admin"];
    public string[] DefaultPermissions { get; set; } =
    [
        "Users.Read", "Users.Write",
        "Roles.Read", "Roles.Write",
        "Permissions.Read", "Permissions.Write"
    ];
    public string[] DefaultDataLabels { get; set; } = ["INTERNAL", "CONFIDENTIAL", "RESTRICTED"];
}
