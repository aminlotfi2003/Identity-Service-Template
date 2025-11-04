using IdentityService.Application.Abstractions.Time;

namespace IdentityService.Infrastructure.CrossCutting.Time;

public sealed class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
