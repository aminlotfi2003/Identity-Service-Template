using IdentityService.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Auth.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutCommandHandler(SignInManager<ApplicationUser> signInManager)
        => _signInManager = signInManager;

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        return Unit.Value;
    }
}
