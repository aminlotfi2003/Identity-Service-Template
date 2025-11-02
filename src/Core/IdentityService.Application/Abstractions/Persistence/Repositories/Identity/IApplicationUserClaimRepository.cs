using IdentityService.Domain.Identity;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Identity;

public interface IApplicationUserClaimRepository : IRepository<ApplicationUserClaim, int>
{
    Task<IReadOnlyList<ApplicationUserClaim>> ListByUserAsync(Guid userId, CancellationToken ct = default);
}
