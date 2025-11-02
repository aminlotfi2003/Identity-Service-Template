using IdentityService.Domain.Common;

namespace IdentityService.Domain.Authorization;

public sealed class Permission : EntityBase<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Category { get; set; }
}
