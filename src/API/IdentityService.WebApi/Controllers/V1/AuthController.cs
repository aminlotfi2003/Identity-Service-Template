using Asp.Versioning;
using IdentityService.Application.Auth.Commands.Login;
using IdentityService.Application.Auth.Commands.Logout;
using IdentityService.Application.Auth.Commands.TwoFactor;
using IdentityService.Application.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    public AuthController(ISender mediator) => _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command, ct));

    [HttpPost("2fa/verify")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Verify2Fa([FromBody] VerifyAuthenticatorCodeCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command, ct));

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        await _mediator.Send(new LogoutCommand(), ct);
        return NoContent();
    }
}
