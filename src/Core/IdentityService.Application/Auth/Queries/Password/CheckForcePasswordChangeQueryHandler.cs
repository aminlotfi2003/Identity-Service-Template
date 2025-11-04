using IdentityService.Application.Abstractions.Context;
using IdentityService.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Auth.Queries.Password;

public sealed class CheckForcePasswordChangeQueryHandler : IRequestHandler<CheckForcePasswordChangeQuery, bool>
{
    private readonly ICurrentUserContext _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckForcePasswordChangeQueryHandler(
        ICurrentUserContext currentUser,
        UserManager<ApplicationUser> userManager)
    {
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public async Task<bool> Handle(CheckForcePasswordChangeQuery request, CancellationToken ct)
    {
        if (_currentUser.UserId is null) return false;

        var user = await _userManager.FindByIdAsync(_currentUser.UserId.Value.ToString());
        return user?.MustChangePasswordOnFirstLogin ?? false;
    }
}
