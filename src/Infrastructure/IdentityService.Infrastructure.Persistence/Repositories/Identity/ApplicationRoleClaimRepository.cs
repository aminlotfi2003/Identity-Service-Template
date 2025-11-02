using IdentityService.Application.Abstractions.Persistence.Repositories.Identity;
using IdentityService.Domain.Identity;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Identity;

public sealed class ApplicationRoleClaimRepository : Repository<ApplicationRoleClaim, int>, IApplicationRoleClaimRepository
{
    public ApplicationRoleClaimRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<ApplicationRoleClaim>> ListByRoleAsync(Guid roleId, CancellationToken ct = default)
        => await _set.Where(x => x.RoleId == roleId).ToListAsync(ct);
}
