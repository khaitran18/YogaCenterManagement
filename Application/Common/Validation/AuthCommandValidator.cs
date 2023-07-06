using Application.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validation
{
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username must not be empty")
                .MinimumLength(6).WithMessage("Username must be at least 6 characters")
                .MaximumLength(30).WithMessage("Username must be less than 30 characters");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must not be empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(40).WithMessage("Password must be less than 40 characters");

        }
    }
}
