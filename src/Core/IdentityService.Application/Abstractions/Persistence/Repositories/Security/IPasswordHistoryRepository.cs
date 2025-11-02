using IdentityService.Domain.Security;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Security;

public interface IPasswordHistoryRepository : IRepository<PasswordHistory, Guid>
{
    Task<IReadOnlyList<string>> GetRecentHashesAsync(Guid userId, int take, CancellationToken ct = default);
}
