using IdentityService.Domain.Common;

namespace IdentityService.Domain.Authorization;

public sealed class RolePermission : EntityBase<Guid>
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
