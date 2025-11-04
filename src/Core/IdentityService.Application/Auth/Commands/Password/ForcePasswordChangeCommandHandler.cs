using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Application.Auth.Dtos;
using IdentityService.Domain.Identity;
using IdentityService.Domain.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Auth.Commands.Password;

public sealed class ForcePasswordChangeCommandHandler : IRequestHandler<ForcePasswordChangeCommand, ChangePasswordResultDto>
{
    private readonly ICurrentUserContext _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordHistoryRepository _passwordHistory;
    private readonly IUnitOfWork _uow;

    public ForcePasswordChangeCommandHandler(
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

    public async Task<ChangePasswordResultDto> Handle(ForcePasswordChangeCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return new ChangePasswordResultDto { Succeeded = false, Message = "کاربر احراز هویت نشده است." };

        var user = await _userManager.FindByIdAsync(_currentUser.UserId.Value.ToString());
        if (user is null || !user.IsActive)
            return new ChangePasswordResultDto { Succeeded = false, Message = "کاربر معتبر نیست." };

        if (user.MustChangePasswordOnFirstLogin != true)
            return new ChangePasswordResultDto { Succeeded = false, Message = "این کاربر نیازی به تغییر رمز اجباری ندارد." };

        var hashed = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
        user.PasswordHash = hashed;
        user.MustChangePasswordOnFirstLogin = false;
        user.PasswordLastChangedAt = DateTimeOffset.UtcNow;

        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
            return new ChangePasswordResultDto
            {
                Succeeded = false,
                Message = string.Join(" | ", update.Errors.Select(e => e.Description))
            };

        await _passwordHistory.AddAsync(new PasswordHistory
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            PasswordHash = hashed,
            ChangedAt = DateTimeOffset.UtcNow
        }, ct);

        await _uow.SaveChangesAsync(ct);

        return new ChangePasswordResultDto
        {
            Succeeded = true,
            Message = "کلمه عبور با موفقیت تغییر یافت. اکنون می‌توانید وارد سیستم شوید."
        };
    }
}
