using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Persistence.Repositories.Audit;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Application.Auth.Dtos;
using IdentityService.Domain.Audit;
using IdentityService.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserLoginHistoryRepository _loginHistory;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentClientContext _client;

    public LoginCommandHandler(
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

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken ct)
    {
        // Find user by username or email (normalized internally by UserManager)
        var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
                   ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        if (user is null || !user.IsActive)
        {
            await LogAttemptAsync(null, success: false, ct);
            return new LoginResultDto { Succeeded = false };
        }

        // Tenant check
        if (request.TenantId.HasValue && user.TenantId != request.TenantId)
        {
            await LogAttemptAsync(user.Id, success: false, ct);
            return new LoginResultDto { Succeeded = false };
        }

        var beforeFailedCount = await _userManager.GetAccessFailedCountAsync(user);

        // Respect lockout & password rules
        var result = await _signInManager.PasswordSignInAsync(
            user, request.Password,
            isPersistent: request.RememberMe,
            lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            return new LoginResultDto
            {
                Succeeded = false,
                RequiresTwoFactor = true,
                DisplayName = user.UserName,
                UserId = user.Id.ToString()
            };
        }

        if (result.IsLockedOut)
        {
            await LogAttemptAsync(user.Id, success: false, ct);
            return new LoginResultDto { Succeeded = false, IsLockedOut = true };
        }

        if (!result.Succeeded)
        {
            await LogAttemptAsync(user.Id, success: false, ct);
            return new LoginResultDto { Succeeded = false };
        }

        // Must change password?
        var mustChange = user.MustChangePasswordOnFirstLogin == true;

        await LogAttemptAsync(user.Id, success: true, ct, failureCountBeforeSuccess: beforeFailedCount);

        var previous = await _loginHistory.GetRecentAsync(user.Id, 3, ct);

        return new LoginResultDto
        {
            Succeeded = true,
            MustChangePassword = mustChange,
            DisplayName = user.UserName,
            UserId = user.Id.ToString(),
            FailedCountBeforeLastSuccess = beforeFailedCount,
            PreviousLogins = previous.Select(x => new PreviousLoginDto
            {
                OccurredAt = x.OccurredAt,
                IpAddress = x.IpAddress,
                Host = x.Host,
                Success = x.Success
            }).ToList()
        };
    }

    private async Task LogAttemptAsync(Guid? userId, bool success, CancellationToken ct, int failureCountBeforeSuccess = 0)
    {
        var entry = new UserLoginHistory
        {
            Id = Guid.NewGuid(),
            UserId = userId ?? Guid.Empty,
            OccurredAt = DateTimeOffset.UtcNow,
            IpAddress = _client.IpAddress,
            Host = _client.Host,
            Success = success,
            FailureCountBeforeSuccess = failureCountBeforeSuccess
        };
        await _loginHistory.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
