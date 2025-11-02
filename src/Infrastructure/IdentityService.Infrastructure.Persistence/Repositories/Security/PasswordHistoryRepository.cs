using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Domain.Security;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Security;

public sealed class PasswordHistoryRepository : Repository<PasswordHistory, Guid>, IPasswordHistoryRepository
{
    public PasswordHistoryRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<string>> GetRecentHashesAsync(Guid userId, int take, CancellationToken ct = default)
        => await _set.Where(x => x.UserId == userId)
                     .OrderByDescending(x => x.ChangedAt)
                     .Take(take)
                     .Select(x => x.PasswordHash)
                     .ToListAsync(ct);
}
