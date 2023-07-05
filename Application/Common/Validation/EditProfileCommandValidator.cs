using FluentValidation;
using Application.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validation
{
    public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileCommandValidator()
        {
            RuleFor(command => command.FullName)
                .NotEmpty().WithMessage("Full Name is required")
                .MinimumLength(10).WithMessage("Full Name must have at least 10 characters");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be in a valid format");
        }
    }
}
