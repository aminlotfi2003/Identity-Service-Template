using FluentValidation;

namespace IdentityService.Application.Auth.Commands.TwoFactor;

public sealed class VerifyAuthenticatorCodeCommandValidator : AbstractValidator<VerifyAuthenticatorCodeCommand>
{
    public VerifyAuthenticatorCodeCommandValidator()
    {
        RuleFor(x => x.UserNameOrEmail).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Code).NotEmpty().Length(6, 8);
    }
}
