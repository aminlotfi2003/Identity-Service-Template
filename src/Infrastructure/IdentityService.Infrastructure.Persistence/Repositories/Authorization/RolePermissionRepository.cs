using IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;
using IdentityService.Domain.Authorization;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Authorization;

public sealed class RolePermissionRepository : Repository<RolePermission, Guid>, IRolePermissionRepository
{
    public RolePermissionRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<RolePermission>> ListByRoleAsync(Guid roleId, CancellationToken ct = default)
        => await _set.Where(x => x.RoleId == roleId).ToListAsync(ct);

    public Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken ct = default)
        => _set.AnyAsync(x => x.RoleId == roleId && x.PermissionId == permissionId, ct);
}
