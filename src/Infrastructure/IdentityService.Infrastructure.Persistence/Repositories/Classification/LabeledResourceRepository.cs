using IdentityService.Application.Abstractions.Persistence.Repositories.Classification;
using IdentityService.Domain.Classification;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Classification;

public sealed class LabeledResourceRepository : Repository<LabeledResource, Guid>, ILabeledResourceRepository
{
    public LabeledResourceRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<LabeledResource>> ListByResourceAsync(string resourceType, Guid resourceId, CancellationToken ct = default)
    => await _set.Where(x => x.ResourceType == resourceType && x.ResourceId == resourceId).ToListAsync(ct);

    public Task<bool> ExistsAsync(string resourceType, Guid resourceId, Guid labelId, CancellationToken ct = default)
        => _set.AnyAsync(x => x.ResourceType == resourceType && x.ResourceId == resourceId && x.LabelId == labelId, ct);
}
