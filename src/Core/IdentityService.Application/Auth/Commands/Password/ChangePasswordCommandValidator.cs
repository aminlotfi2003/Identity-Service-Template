using FluentValidation;

namespace IdentityService.Application.Auth.Commands.Password;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().MinimumLength(6);
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(12);
        RuleFor(x => x).Must(x => x.CurrentPassword != x.NewPassword)
            .WithMessage("کلمه عبور جدید نباید با قبلی یکسان باشد.");
    }
}
