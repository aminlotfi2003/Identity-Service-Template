namespace IdentityService.Application.Abstractions.Persistence.Repositories;

public interface IRepository<T, TId> :
    IReadRepository<T, TId>,
    IWriteRepository<T, TId>
    where T : class
{ }
