using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Application.Auth.Dtos;
using IdentityService.Domain.Identity;
using IdentityService.Domain.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Auth.Commands.Password;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResultDto>
{
    private readonly ICurrentUserContext _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordHistoryRepository _passwordHistory;
    private readonly IUnitOfWork _uow;

    public ChangePasswordCommandHandler(
        ICurrentUserContext currentUser,
        UserManager<ApplicationUser> userManager,
        IPasswordHistoryRepository passwordHistory,
        IUnitOfWork uow)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _passwordHistory = passwordHistory;
        _uow = uow;
    }

    public async Task<ChangePasswordResultDto> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return new ChangePasswordResultDto { Succeeded = false, Message = "کاربر احراز هویت نشده است." };

        var user = await _userManager.FindByIdAsync(_currentUser.UserId.Value.ToString());
        if (user is null || !user.IsActive)
            return new ChangePasswordResultDto { Succeeded = false, Message = "کاربر معتبر نیست." };

        // Levenshtein
        if (Levenshtein(request.CurrentPassword, request.NewPassword) < 4)
            return new ChangePasswordResultDto { Succeeded = false, Message = "کلمه عبور جدید باید حداقل ۴ کاراکتر با قبلی متفاوت باشد." };

        var change = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!change.Succeeded)
            return new ChangePasswordResultDto { Succeeded = false, Message = string.Join(" | ", change.Errors.Select(e => e.Description)) };

        // Update Metadata
        user.PasswordLastChangedAt = DateTimeOffset.UtcNow;
        user.MustChangePasswordOnFirstLogin = false;
        await _userManager.UpdateAsync(user);

        // Save Password History
        var hasher = _userManager.PasswordHasher;
        var newHash = hasher.HashPassword(user, request.NewPassword);
        await _passwordHistory.AddAsync(new PasswordHistory
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            PasswordHash = newHash,
            ChangedAt = DateTimeOffset.UtcNow
        }, ct);

        await _uow.SaveChangesAsync(ct);

        return new ChangePasswordResultDto { Succeeded = true, Message = "کلمه عبور با موفقیت تغییر یافت." };
    }

    // Implementation Levenshtein
    private static int Levenshtein(string a, string b)
    {
        if (a == b) return 0;
        var n = a.Length; var m = b.Length;
        if (n == 0) return m; if (m == 0) return n;
        var d = new int[n + 1, m + 1];
        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for (int j = 0; j <= m; j++) d[0, j] = j;
        for (int i = 1; i <= n; i++)
            for (int j = 1; j <= m; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        return d[n, m];
    }
}
