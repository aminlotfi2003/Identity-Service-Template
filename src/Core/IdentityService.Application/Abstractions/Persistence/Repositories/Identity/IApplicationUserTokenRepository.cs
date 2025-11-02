using IdentityService.Domain.Identity;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Identity;

public interface IApplicationUserTokenRepository : IRepository<ApplicationUserToken, Guid>
{
    Task<IReadOnlyList<ApplicationUserToken>> ListByUserAsync(Guid userId, CancellationToken ct = default);
}
