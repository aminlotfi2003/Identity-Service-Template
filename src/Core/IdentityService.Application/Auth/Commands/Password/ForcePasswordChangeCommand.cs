using IdentityService.Application.Auth.Dtos;
using MediatR;

namespace IdentityService.Application.Auth.Commands.Password;

public sealed record ForcePasswordChangeCommand(
    string NewPassword
) : IRequest<ChangePasswordResultDto>;
