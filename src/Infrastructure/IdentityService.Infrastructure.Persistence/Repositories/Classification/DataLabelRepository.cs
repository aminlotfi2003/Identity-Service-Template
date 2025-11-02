using IdentityService.Application.Abstractions.Persistence.Repositories.Classification;
using IdentityService.Domain.Classification;
using IdentityService.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Classification;

public sealed class DataLabelRepository : Repository<DataLabel, Guid>, IDataLabelRepository
{
    public DataLabelRepository(ApplicationDbContext db) : base(db) { }

    public Task<DataLabel?> GetByNameAsync(string name, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(x => x.Name == name, ct);
}
