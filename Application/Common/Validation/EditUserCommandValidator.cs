using FluentValidation;
using Application.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validation
{
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            RuleFor(command => command.FullName)
                .NotEmpty().WithMessage("Full Name is required")
                .MinimumLength(10).WithMessage("Full Name must have at least 10 characters");
        }
    }
}
