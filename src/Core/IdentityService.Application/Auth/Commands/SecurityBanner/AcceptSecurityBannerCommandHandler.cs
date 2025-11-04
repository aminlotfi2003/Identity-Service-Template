using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Domain.Security;
using MediatR;

namespace IdentityService.Application.Auth.Commands.SecurityBanner;

public sealed class AcceptSecurityBannerCommandHandler : IRequestHandler<AcceptSecurityBannerCommand, bool>
{
    private readonly ICurrentUserContext _currentUser;
    private readonly ISecurityBannerAcceptanceRepository _repository;
    private readonly IUnitOfWork _uow;

    public AcceptSecurityBannerCommandHandler(
        ICurrentUserContext currentUser,
        ISecurityBannerAcceptanceRepository repository,
        IUnitOfWork uow)
    {
        _currentUser = currentUser;
        _repository = repository;
        _uow = uow;
    }

    public async Task<bool> Handle(AcceptSecurityBannerCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return false;

        var userId = _currentUser.UserId.Value;
        var version = request.Version ?? "v1";

        var alreadyAccepted = await _repository.HasAcceptedAsync(userId, version, ct);
        if (alreadyAccepted)
            return true;

        var entity = new SecurityBannerAcceptance
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Version = version,
            AcceptedAt = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
