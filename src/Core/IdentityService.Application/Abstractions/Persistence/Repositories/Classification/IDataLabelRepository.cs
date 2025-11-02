using IdentityService.Domain.Classification;

namespace IdentityService.Application.Abstractions.Persistence.Repositories.Classification;

public interface IDataLabelRepository : IRepository<DataLabel, Guid>
{
    Task<DataLabel?> GetByNameAsync(string name, CancellationToken ct = default);
}
