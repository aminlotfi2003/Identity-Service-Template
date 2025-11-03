namespace IdentityService.Infrastructure.Identity.Options;

public sealed class IdentityTokenSettings
{
    public int DefaultLifespanMinutes { get; set; } = 120; // email confirm / reset
    public TimeSpan DefaultLifespan => TimeSpan.FromMinutes(DefaultLifespanMinutes);
}
