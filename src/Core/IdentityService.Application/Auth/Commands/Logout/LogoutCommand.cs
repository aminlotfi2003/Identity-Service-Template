using MediatR;

namespace IdentityService.Application.Auth.Commands.Logout;

public sealed record LogoutCommand() : IRequest<Unit>;
