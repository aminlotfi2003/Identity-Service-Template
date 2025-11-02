using IdentityService.Domain.Authorization;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;

public interface IUserPermissionRepository : IRepository<UserPermission, Guid>
{
    Task<IReadOnlyList<UserPermission>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid userId, Guid permissionId, CancellationToken ct = default);
}
