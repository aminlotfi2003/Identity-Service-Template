using IdentityService.Domain.Security;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Security;

public interface ISecurityBannerAcceptanceRepository : IRepository<SecurityBannerAcceptance, Guid>
{
    Task<SecurityBannerAcceptance?> GetLatestAsync(Guid userId, CancellationToken ct = default);
}
