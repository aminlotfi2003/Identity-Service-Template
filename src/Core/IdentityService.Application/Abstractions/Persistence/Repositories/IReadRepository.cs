using System.Linq.Expressions;

namespace IdentityService.Application.Abstractions.Persistence.Repositories;

public interface IReadRepository<T, TId> where T : class
{
    Task<T?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

    Task<List<T>> ListAsync(
        Func<IQueryable<T>, IQueryable<T>>? queryShaper = null,
        CancellationToken ct = default);

    Task<T?> FirstOrDefaultAsync(
        Func<IQueryable<T>, IQueryable<T>> queryShaper,
        CancellationToken ct = default);
}
