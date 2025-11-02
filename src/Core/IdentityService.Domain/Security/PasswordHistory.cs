using IdentityService.Domain.Common;

namespace IdentityService.Domain.Security;

public sealed class PasswordHistory : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public string PasswordHash { get; set; } = default!;
    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;
}
