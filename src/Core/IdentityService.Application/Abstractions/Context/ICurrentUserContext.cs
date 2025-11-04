namespace IdentityService.Application.Abstractions.Context;

public interface ICurrentUserContext
{
    Guid? UserId { get; }
    string? UserName { get; }
}
