using IdentityService.Application.Abstractions.Persistence.Repositories.Identity;
using IdentityService.Domain.Identity;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Identity;

public sealed class ApplicationUserClaimRepository : Repository<ApplicationUserClaim, int>, IApplicationUserClaimRepository
{
    public ApplicationUserClaimRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<ApplicationUserClaim>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        => await _set.Where(x => x.UserId == userId).ToListAsync(ct);
}
