using IdentityService.Domain.Authorization;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;

public interface IPermissionRepository : IRepository<Permission, Guid>
{
    Task<Permission?> GetByNameAsync(string name, CancellationToken ct = default);
}
