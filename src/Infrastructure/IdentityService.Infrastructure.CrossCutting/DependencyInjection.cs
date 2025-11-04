using IdentityService.Application.Abstractions.Context;
using IdentityService.Application.Abstractions.Time;
using IdentityService.Infrastructure.CrossCutting.Context;
using IdentityService.Infrastructure.CrossCutting.Time;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.CrossCutting;

public static class DependencyInjection
{
    public static IServiceCollection AddCrossCutting(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentClientContext, CurrentClientContext>();
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddSingleton<ISystemClock, SystemClock>();
        return services;
    }
}
