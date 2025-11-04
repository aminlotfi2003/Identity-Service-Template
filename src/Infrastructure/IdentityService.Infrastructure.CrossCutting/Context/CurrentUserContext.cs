using IdentityService.Application.Abstractions.Context;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityService.Infrastructure.CrossCutting.Context;

public sealed class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _http;
    public CurrentUserContext(IHttpContextAccessor http) => _http = http;

    public Guid? UserId
    {
        get
        {
            var id = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : null;
        }
    }

    public string? UserName => _http.HttpContext?.User?.Identity?.Name;
}
