using Asp.Versioning;
using IdentityService.Application.Auth.Commands.SecurityBanner;
using IdentityService.Application.Auth.Queries.SecurityBanner;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/security")]
public sealed class SecurityController : ControllerBase
{
    private readonly ISender _mediator;
    public SecurityController(ISender mediator) => _mediator = mediator;

    [HttpGet("banner/status")]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBannerStatus([FromQuery] string? version, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSecurityBannerStatusQuery(version), ct));

    [HttpPost("banner/accept")]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> AcceptBanner([FromBody] AcceptSecurityBannerCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command, ct));
}
