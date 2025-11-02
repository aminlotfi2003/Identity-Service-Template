using IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;
using IdentityService.Domain.Authorization;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Authorization;

public sealed class PermissionRepository : Repository<Permission, Guid>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext db) : base(db) { }

    public Task<Permission?> GetByNameAsync(string name, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(x => x.Name == name, ct);
}
