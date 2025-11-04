using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using MediatR;

namespace IdentityService.Application.Auth.Queries.SecurityBanner;

public sealed class GetSecurityBannerStatusQueryHandler : IRequestHandler<GetSecurityBannerStatusQuery, bool>
{
    private readonly ICurrentUserContext _currentUser;
    private readonly ISecurityBannerAcceptanceRepository _repository;

    public GetSecurityBannerStatusQueryHandler(
        ICurrentUserContext currentUser,
        ISecurityBannerAcceptanceRepository repository)
    {
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<bool> Handle(GetSecurityBannerStatusQuery request, CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return false;

        return await _repository.HasAcceptedAsync(_currentUser.UserId.Value, request.Version, ct);
    }
}
