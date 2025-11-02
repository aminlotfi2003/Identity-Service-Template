using IdentityService.Domain.Audit;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Audit;

public interface IUserLoginHistoryRepository : IRepository<UserLoginHistory, Guid>
{
    Task<IReadOnlyList<UserLoginHistory>> GetRecentAsync(Guid userId, int count, CancellationToken ct = default);
}
