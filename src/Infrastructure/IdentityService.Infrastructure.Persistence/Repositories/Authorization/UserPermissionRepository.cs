using IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;
using IdentityService.Domain.Authorization;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Authorization;

public sealed class UserPermissionRepository : Repository<UserPermission, Guid>, IUserPermissionRepository
{
    public UserPermissionRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<UserPermission>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        => await _set.Where(x => x.UserId == userId).ToListAsync(ct);

    public Task<bool> ExistsAsync(Guid userId, Guid permissionId, CancellationToken ct = default)
        => _set.AnyAsync(x => x.UserId == userId && x.PermissionId == permissionId, ct);
}
