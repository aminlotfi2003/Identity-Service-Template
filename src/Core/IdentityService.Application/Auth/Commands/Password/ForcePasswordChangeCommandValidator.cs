using FluentValidation;

namespace IdentityService.Application.Auth.Commands.Password;

public sealed class ForcePasswordChangeCommandValidator : AbstractValidator<ForcePasswordChangeCommand>
{
    public ForcePasswordChangeCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(12)
            .WithMessage("کلمه عبور جدید باید حداقل ۱۲ کاراکتر باشد.");
    }
}
