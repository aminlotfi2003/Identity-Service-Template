using IdentityService.Application.Abstractions.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories.Base;

public class Repository<T, TId> : IRepository<T, TId> where T : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _set;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(TId id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public virtual async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _set.AnyAsync(predicate, ct);

    public virtual async Task<long> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => predicate is null ? await _set.LongCountAsync(ct) : await _set.LongCountAsync(predicate, ct);

    public virtual async Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? queryShaper = null, CancellationToken ct = default)
        => await (queryShaper is null ? _set : queryShaper(_set)).ToListAsync(ct);

    public virtual async Task<T?> FirstOrDefaultAsync(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken ct = default)
        => await queryShaper(_set).FirstOrDefaultAsync(ct);

    public virtual async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _set.AddAsync(entity, ct);
        return entity;
    }

    public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => _set.AddRangeAsync(entities, ct);

    public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _set.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(T entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        _set.RemoveRange(entities);
        return Task.CompletedTask;
    }
}
