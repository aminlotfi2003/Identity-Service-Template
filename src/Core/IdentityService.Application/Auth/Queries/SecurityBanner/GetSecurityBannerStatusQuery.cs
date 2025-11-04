using MediatR;

namespace IdentityService.Application.Auth.Queries.SecurityBanner;

public sealed record GetSecurityBannerStatusQuery(string? Version = "v1") : IRequest<bool>;
