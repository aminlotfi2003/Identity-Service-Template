namespace IdentityService.Application.Auth.Dtos;

public sealed class LoginResultDto
{
    public bool Succeeded { get; init; }
    public bool RequiresTwoFactor { get; init; }
    public bool IsLockedOut { get; init; }
    public bool MustChangePassword { get; init; }
    public bool PasswordExpired { get; init; }
    public string? DisplayName { get; init; }
    public string? UserId { get; init; }

    public IReadOnlyList<PreviousLoginDto> PreviousLogins { get; init; } = Array.Empty<PreviousLoginDto>();
    public int FailedCountBeforeLastSuccess { get; init; }
}
