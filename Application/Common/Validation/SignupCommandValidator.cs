
using Application.Command;
using FluentValidation;

namespace Application.Common.Validation
{
    public class SignupCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignupCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username must not be empty")
                .MinimumLength(6).WithMessage("Username must be at least 6 characters")
                .MaximumLength(30).WithMessage("Username must be less than 30 characters");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must not be empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(40).WithMessage("Password must be less than 40 characters")
                .Matches("(?=.*[A-Z])(?=.*\\d)").WithMessage("Password must contain at least one uppercase letter and one digit");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must not be empty")
                .EmailAddress().WithMessage("Email be the correct syntax");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Fullname must not be empty");
        }
    }
}
