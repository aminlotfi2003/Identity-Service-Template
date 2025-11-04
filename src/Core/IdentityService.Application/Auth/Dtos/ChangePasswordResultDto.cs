namespace IdentityService.Application.Auth.Dtos;

public sealed class ChangePasswordResultDto
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
}
