using Asp.Versioning;
using IdentityService.Application.Auth.Commands.Password;
using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Queries.Password;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/account")]
public sealed class AccountController : ControllerBase
{
    private readonly ISender _mediator;
    public AccountController(ISender mediator) => _mediator = mediator;

    // POST: api/v1/account/password/change
    [HttpPost("password/change")]
    [Authorize]
    [ProducesResponseType(typeof(ChangePasswordResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand cmd, CancellationToken ct)
        => Ok(await _mediator.Send(cmd, ct));

    // POST: api/v1/account/password/force-change
    [HttpPost("password/force-change")]
    [Authorize]
    [ProducesResponseType(typeof(ChangePasswordResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForceChangePassword([FromBody] ForcePasswordChangeCommand cmd, CancellationToken ct)
        => Ok(await _mediator.Send(cmd, ct));

    // GET: api/v1/account/password/must-change
    [HttpGet("password/must-change")]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> MustChangePassword(CancellationToken ct)
        => Ok(await _mediator.Send(new CheckForcePasswordChangeQuery(), ct));
}
