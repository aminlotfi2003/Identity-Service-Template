using IdentityService.Application.Abstractions.Persistence.Repositories.Identity;
using IdentityService.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Identity;

public sealed class ApplicationUserRoleLinkRepository : IApplicationUserRoleLinkRepository
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<ApplicationUserRole> _set;

    public ApplicationUserRoleLinkRepository(ApplicationDbContext db)
    {
        _db = db;
        _set = db.Set<ApplicationUserRole>();
    }

    public Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken ct = default)
        => _set.AnyAsync(x => x.UserId == userId && x.RoleId == roleId, ct);

    public async Task AddAsync(Guid userId, Guid roleId, CancellationToken ct = default)
        => await _set.AddAsync(new ApplicationUserRole { UserId = userId, RoleId = roleId }, ct);

    public async Task RemoveAsync(Guid userId, Guid roleId, CancellationToken ct = default)
    {
        var entity = await _set.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, ct);
        if (entity is not null) _set.Remove(entity);
    }
}
