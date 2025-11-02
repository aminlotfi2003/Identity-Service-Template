using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Domain.Security;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Security;

public sealed class SecurityBannerAcceptanceRepository : Repository<SecurityBannerAcceptance, Guid>, ISecurityBannerAcceptanceRepository
{
    public SecurityBannerAcceptanceRepository(ApplicationDbContext db) : base(db) { }

    public Task<SecurityBannerAcceptance?> GetLatestAsync(Guid userId, CancellationToken ct = default)
        => _set.Where(x => x.UserId == userId)
               .OrderByDescending(x => x.AcceptedAt)
               .FirstOrDefaultAsync(ct);
}
