using MediatR;

namespace IdentityService.Application.Auth.Commands.SecurityBanner;

public sealed record AcceptSecurityBannerCommand(string? Version = "v1") : IRequest<bool>;
