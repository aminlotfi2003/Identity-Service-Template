namespace IdentityService.Application.Abstractions.Persistence.Repositories.Identity;

public interface IApplicationUserRoleLinkRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken ct = default);
    Task AddAsync(Guid userId, Guid roleId, CancellationToken ct = default);
    Task RemoveAsync(Guid userId, Guid roleId, CancellationToken ct = default);
}
