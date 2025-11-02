using IdentityService.Domain.Classification;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Classification;

public interface ILabeledResourceRepository : IRepository<LabeledResource, Guid>
{
    Task<IReadOnlyList<LabeledResource>> ListByResourceAsync(string resourceType, Guid resourceId, CancellationToken ct = default);
    Task<bool> ExistsAsync(string resourceType, Guid resourceId, Guid labelId, CancellationToken ct = default);
}
