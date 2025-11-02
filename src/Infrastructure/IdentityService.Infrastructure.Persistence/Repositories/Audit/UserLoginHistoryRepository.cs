using IdentityService.Application.Abstractions.Persistence.Repositories.Audit;
using IdentityService.Domain.Audit;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Audit;

public sealed class UserLoginHistoryRepository : Repository<UserLoginHistory, Guid>, IUserLoginHistoryRepository
{
    public UserLoginHistoryRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<UserLoginHistory>> GetRecentAsync(Guid userId, int count, CancellationToken ct = default)
        => await _set.Where(x => x.UserId == userId)
                     .OrderByDescending(x => x.OccurredAt)
                     .Take(count)
                     .ToListAsync(ct);
}
