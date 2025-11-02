using IdentityService.Domain.Common;

namespace IdentityService.Domain.Authorization;

public sealed class UserPermission : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }
}
