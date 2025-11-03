using IdentityService.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IdentityService.Infrastructure.Identity.Claims;

public sealed class ApplicationUserClaimsPrincipalFactory
    : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    public ApplicationUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var id = await base.GenerateClaimsAsync(user);

        if (user.TenantId is Guid tenantId)
            id.AddClaim(new Claim(IdentityClaimTypes.TenantId, tenantId.ToString()));

        id.AddClaim(new Claim(IdentityClaimTypes.IsActive, user.IsActive ? "true" : "false"));

        if (!id.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        return id;
    }
}
