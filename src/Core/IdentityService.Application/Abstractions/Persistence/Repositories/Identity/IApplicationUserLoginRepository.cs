using IdentityService.Domain.Identity;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Identity;

public interface IApplicationUserLoginRepository : IRepository<ApplicationUserLogin, string>
{
    Task<IReadOnlyList<ApplicationUserLogin>> ListByUserAsync(Guid userId, CancellationToken ct = default);
}
