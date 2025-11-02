using IdentityService.Domain.Common;

namespace IdentityService.Domain.Security;

public sealed class SecurityBannerAcceptance : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public string Version { get; set; } = "v1";
    public DateTimeOffset AcceptedAt { get; set; } = DateTimeOffset.UtcNow;
}
