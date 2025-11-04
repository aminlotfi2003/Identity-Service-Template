namespace IdentityService.Application.Abstractions.Context;

public interface ICurrentClientContext
{
    string? IpAddress { get; }
    string? Host { get; }
}
