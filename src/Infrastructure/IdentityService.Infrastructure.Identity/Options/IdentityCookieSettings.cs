namespace IdentityService.Infrastructure.Identity.Options;

public sealed class IdentityCookieSettings
{
    public string Name { get; set; } = ".ids.auth";
    public string LoginPath { get; set; } = "/account/login";
    public string LogoutPath { get; set; } = "/account/logout";
    public string AccessDeniedPath { get; set; } = "/errors/403";
    public int ExpireMinutes { get; set; } = 20;
    public TimeSpan ExpireTimeSpan => TimeSpan.FromMinutes(ExpireMinutes);
}
