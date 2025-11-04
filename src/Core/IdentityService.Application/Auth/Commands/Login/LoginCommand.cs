using IdentityService.Application.Auth.Dtos;
using MediatR;

namespace IdentityService.Application.Auth.Commands.Login;

public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password,
    bool RememberMe,
    Guid? TenantId = null
) : IRequest<LoginResultDto>;
