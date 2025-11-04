namespace IdentityService.Application.Auth.Dtos;

public sealed class PreviousLoginDto
{
    public DateTimeOffset OccurredAt { get; init; }
    public string? IpAddress { get; init; }
    public string? Host { get; init; }
    public bool Success { get; init; }
}
