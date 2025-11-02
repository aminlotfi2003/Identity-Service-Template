using IdentityService.Domain.Identity;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Identity;

public interface IApplicationRoleClaimRepository : IRepository<ApplicationRoleClaim, int>
{
    Task<IReadOnlyList<ApplicationRoleClaim>> ListByRoleAsync(Guid roleId, CancellationToken ct = default);
}
