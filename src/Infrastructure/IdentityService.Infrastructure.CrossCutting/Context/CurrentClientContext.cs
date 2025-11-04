using IdentityService.Application.Abstractions.Context;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Infrastructure.CrossCutting.Context;

public sealed class CurrentClientContext : ICurrentClientContext
{
    private readonly IHttpContextAccessor _http;
    public CurrentClientContext(IHttpContextAccessor http) => _http = http;

    public string? IpAddress =>
        _http.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? Host =>
        _http.HttpContext?.Request?.Host.Value;
}
