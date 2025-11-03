using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Identity.Validators;

public sealed class PasswordHistoryValidator : IPasswordValidator<ApplicationUser>
{
    private readonly IPasswordHistoryRepository _history;
    private readonly IPasswordHasher<ApplicationUser> _hasher;
    private const int DefaultHistoryCount = 5;
    private const int MinDiffChars = 4;

    public PasswordHistoryValidator(IPasswordHistoryRepository history, IPasswordHasher<ApplicationUser> hasher)
    {
        _history = history;
        _hasher = hasher;
    }

    public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
    {
        var errors = new List<IdentityError>();

        var recent = await _history.GetRecentHashesAsync(user.Id, DefaultHistoryCount, CancellationToken.None);
        foreach (var oldHash in recent)
        {
            var result = _hasher.VerifyHashedPassword(user, oldHash, password);
            if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                errors.Add(new IdentityError { Code = "PasswordReused", Description = "این کلمه عبور قبلاً استفاده شده است." });
                break;
            }
        }

        var lastHash = recent.FirstOrDefault();
        if (lastHash is not null)
        {
            
        }

        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed([.. errors]);
    }
}
