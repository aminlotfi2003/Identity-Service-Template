using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Persistence.Repositories.Audit;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Application.Auth.Dtos;
using IdentityService.Domain.Audit;
using IdentityService.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Auth.Commands.TwoFactor;

public sealed class VerifyAuthenticatorCodeCommandHandler : IRequestHandler<VerifyAuthenticatorCodeCommand, LoginResultDto>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserLoginHistoryRepository _loginHistory;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentClientContext _client;

    public VerifyAuthenticatorCodeCommandHandler(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IUserLoginHistoryRepository loginHistory,
        IUnitOfWork uow,
        ICurrentClientContext client)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _loginHistory = loginHistory;
        _uow = uow;
        _client = client;
    }

    public async Task<LoginResultDto> Handle(VerifyAuthenticatorCodeCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
                   ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        if (user is null || !user.IsActive)
            return new LoginResultDto { Succeeded = false };

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
            request.Code, request.RememberMe, request.RememberMachine);

        if (!result.Succeeded)
        {
            await LogAttemptAsync(user.Id, success: false, ct);
            if (result.IsLockedOut)
                return new LoginResultDto { Succeeded = false, IsLockedOut = true };
            return new LoginResultDto { Succeeded = false };
        }

        await LogAttemptAsync(user.Id, success: true, ct);

        var previous = await _loginHistory.GetRecentAsync(user.Id, 3, ct);

        return new LoginResultDto
        {
            Succeeded = true,
            DisplayName = user.UserName,
            UserId = user.Id.ToString(),
            PreviousLogins = previous.Select(x => new PreviousLoginDto
            {
                OccurredAt = x.OccurredAt,
                IpAddress = x.IpAddress,
                Host = x.Host,
                Success = x.Success
            }).ToList()
        };
    }

    private async Task LogAttemptAsync(Guid userId, bool success, CancellationToken ct)
    {
        var entry = new UserLoginHistory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OccurredAt = DateTimeOffset.UtcNow,
            IpAddress = _client.IpAddress,
            Host = _client.Host,
            Success = success,
            FailureCountBeforeSuccess = 0
        };
        await _loginHistory.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
