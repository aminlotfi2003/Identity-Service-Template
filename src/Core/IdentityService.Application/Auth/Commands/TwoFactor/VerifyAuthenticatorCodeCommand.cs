using IdentityService.Application.Auth.Dtos;
using MediatR;

namespace IdentityService.Application.Auth.Commands.TwoFactor;

public sealed record VerifyAuthenticatorCodeCommand(
    string UserNameOrEmail,
    string Code,
    bool RememberMe,
    bool RememberMachine
) : IRequest<LoginResultDto>;
