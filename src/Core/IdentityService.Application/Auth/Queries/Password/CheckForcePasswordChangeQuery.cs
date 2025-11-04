using MediatR;

namespace IdentityService.Application.Auth.Queries.Password;

public sealed record CheckForcePasswordChangeQuery() : IRequest<bool>;
