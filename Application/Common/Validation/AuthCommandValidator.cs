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
                .NotEmpty().WithMessage("Username must not be empty");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must not be empty");

        }
    }
}
