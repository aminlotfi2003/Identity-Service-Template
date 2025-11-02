using IdentityService.Domain.Authorization;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;

public interface IRolePermissionRepository : IRepository<RolePermission, Guid>
{
    Task<IReadOnlyList<RolePermission>> ListByRoleAsync(Guid roleId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken ct = default);
}
